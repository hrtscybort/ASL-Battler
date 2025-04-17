using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TensorFlowLite;
using System.IO;
using System.Linq;
using Common;
using UnityEngine.Networking;



// Create a new type of TFLiteModelManager to specifically deal with the phonology models
namespace Model {
	public abstract class TfLiteModelManagerPhonology {
		protected Interpreter interpreter;
		protected int numThreads = 1;

		protected int maxFrames;
		protected int inputSize;
		protected int glossOutputSize;
		protected int handshapeOutputSize;

		protected int minorLocationOutputSize;

		protected TextAsset model;

		//protected List<T> mapping;



		protected Interpreter GetInterpreter() {
			if (interpreter == null) {
				interpreter = new Interpreter(model.bytes, new InterpreterOptions()
				{
					threads = numThreads,
				});

				maxFrames = interpreter.GetInputTensorInfo(0).shape[1];
				inputSize = interpreter.GetInputTensorInfo(0).shape[2];

				// I am taking a guess this is correct based off of the tflite tests
				glossOutputSize = interpreter.GetOutputTensorInfo(1).shape[1];
				handshapeOutputSize = interpreter.GetOutputTensorInfo(0).shape[1];
				minorLocationOutputSize = interpreter.GetOutputTensorInfo(2).shape[1];
			}

			return interpreter;
		}
		// since data can be dynamic - have multiple tensors etc - we don't have a Run Model function
		// as that would require different signatures.
	}

	public class SLRPhonemeModel : TfLiteModelManagerPhonology {
		// float[,,,] modelInputTensor;
		// NativeArray<float> modelOutputTensor;
		// NativeArray<float> modelOutputTensor;
		
		private Dictionary<string, Action<PhonemePrediction>> callbacks = new();
		
		public List<PredictionFilter<string>> outputFilters = new();
		
		private readonly float[] glossOutputs = new float[506]; // 506 merged sign categories
		private readonly float[] handshapeOutputs = new float[41]; // 41 different handshape
		private readonly float[] minorLocationOutputs = new float[32]; // 32 different minor locations

		private float[] inputs = new float[42 * 80]; // 42 features * 80 frames

		private Dictionary<int, string> idx_to_category;
        private Dictionary<int, string> idx_to_handshape;
        private Dictionary<int, string> idx_to_minor_loc;

		//private List<string> gloss_mapping;

        private Dictionary<string, string> category_to_handshape;

        private Dictionary<string, string> category_to_minor_location;

		public SLRPhonemeModel(TextAsset model, Dictionary<int, string> idx_to_category, 
								Dictionary<int, string> idx_to_handshape, Dictionary<int, string> idx_to_minor_loc) {
			this.model = model;
			GetInterpreter();
			interpreter.AllocateTensors();
			// modelInputTensor = new float[1, maxFrames, inputSize, 1];
			// modelOutputTensor = new NativeArray<float>(outputSize, Allocator.Persistent);
			
			//this.metadata = metadata; // Assume we are able to read in the metadata
			//outputFilters.Add(new PassThroughFilterSingle<T>());

			this.idx_to_category = idx_to_category;
			this.idx_to_handshape = idx_to_handshape;
			this.idx_to_minor_loc = idx_to_minor_loc;


			// Apply filters on the gloss output
			
		}

		public void RunModel(float[] data) {
			// Debug.Log("Data: " + data.Length + " -> " + string.Join(", ", data));
			Array.Copy(data, inputs, data.Length);
			interpreter.SetInputTensorData(0, inputs);
			// Debug.Log("invoke");
			interpreter.Invoke();

			// interpreter.GetOutputTensorData(0, modelOutputTensor.AsSpan());
			
			// Extract the output data from the model
			interpreter.GetOutputTensorData(1, glossOutputs);
			interpreter.GetOutputTensorData(0, handshapeOutputs);
			interpreter.GetOutputTensorData(2, minorLocationOutputs);

			float[] sendGlossOutputs = new float[glossOutputs.Length];
			Array.Copy(glossOutputs, sendGlossOutputs, glossOutputs.Length);
			// Debug.Log("outputs: " + string.Join(", ", sendOutputs));
			// float[] outputs = modelOutputTensor.ToArray();

			List<string> gloss_mapping = new List<string>();
			for (int i = 0; i < 506; i++) {
				gloss_mapping.Add(idx_to_category[i]);
			}

			FilterUnit<string> glossOutput = new FilterUnit<string>(gloss_mapping, sendGlossOutputs);
			foreach (var filter in outputFilters)
			{
				glossOutput = filter.Filter(glossOutput);
			}
			


			//Extract the output indicies from each result
			
			//int glossIndex = MathUtil.Argmax(new List<float>(glossOutputs));

			List<float> glossOutputList = glossOutput.probabilities.ToList();
			List<float> handshapeOutputsList = new List<float>(handshapeOutputs);
			List<float> minorLocationOutputsList = new List<float>(minorLocationOutputs);

			int glossIndex = 0;
			int handshapeIndex = 0;
			int minorLocationIndex = 0;

			if (glossOutputList.Count >= 1) {
				glossIndex = MathUtil.Argmax(glossOutputList);
			} else {
				Debug.Log("Handshape outputs count is less than or equal to 1. List: " + glossOutputList + " Output: " + glossOutputs);
				foreach (var callback in callbacks) {
					callbacks[callback.Key].Invoke(new PhonemePrediction("None", "None", "None"));
				}
				return;
			}

			if (handshapeOutputsList.Count >= 1) {
				handshapeIndex = MathUtil.Argmax(new List<float>(handshapeOutputs));
			} else {
				Debug.Log("Handshape outputs count is less than or equal to 1. List: " + handshapeOutputsList + " Output: " + handshapeOutputs);
				foreach (var callback in callbacks) {
					callbacks[callback.Key].Invoke(new PhonemePrediction("None", "None", "None"));
				}
				return;
			}

			if (minorLocationOutputsList.Count >= 1) {
				minorLocationIndex = MathUtil.Argmax(new List<float>(minorLocationOutputs));
			} else {
				Debug.Log("Minor Location outputs count is less than or equal to 1. List: " + minorLocationOutputsList + " Output: " + minorLocationOutputs);
				foreach (var callback in callbacks) {
					callbacks[callback.Key].Invoke(new PhonemePrediction("None", "None", "None"));
				}
				return;
			}




			//string glossString = this.idx_to_category[glossIndex];

			
			string glossString = glossOutput.mapping[glossIndex];
			string handshapeString = this.idx_to_handshape[handshapeIndex];
			string minorLocationString = this.idx_to_minor_loc[minorLocationIndex];

			// Create the phonemePrediction object to return

			PhonemePrediction phonemePrediction = new PhonemePrediction(glossString, handshapeString, minorLocationString);

			// Put return values for each callback that the user may have made
			foreach (var callback in callbacks) {

				callbacks[callback.Key].Invoke(phonemePrediction);
				/*
				if (output.mapping.Count == 1) {
					callbacks[callback.Key].Invoke(output.mapping[0]);
				}
				else if (output.mapping.Count > 1) {
					callbacks[callback.Key].Invoke(output.mapping[MathUtil.Argmax(output.probabilities.ToList())]);
				}
				*/
			}
			// modelOutputTensor.Dispose();
		}
		
		public void AddCallback(string name, Action<PhonemePrediction> callback) {
			if (callbacks.ContainsKey(name)) callbacks.Remove(name);
			callbacks.Add(name, callback);
		}

		public void RemoveCallback(string name) {
			callbacks.Remove(name);
		}


		public string toString() {
			return "SLRPhoneme model. Callbacks: " + this.callbacks;
		}
	}
}

public struct PhonemePrediction
{
    public string gloss;
    public string handshape;
    public string minorLocation;

	public PhonemePrediction(string gloss, string handshape, string minorLocation) {
		this.gloss = gloss;
		this.handshape = handshape;
		this.minorLocation = minorLocation;
	}

	public override string ToString() => $"(Gloss: {gloss}, Handshape: {handshape}, Minor Location {minorLocation})";
}