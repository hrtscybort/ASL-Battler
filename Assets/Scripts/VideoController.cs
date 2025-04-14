using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoClip[] tutorialVideos;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private TMP_Text word;
    
    private int currVideoIndex = 0;

    private void Start()
    {
        PlayVideo(currVideoIndex);

        nextButton.onClick.AddListener(NextVideo);
        prevButton.onClick.AddListener(PreviousVideo);
    }

    private void PlayVideo(int index)
    {
        if (index < 0 || index >= tutorialVideos.Length) return;
        videoPlayer.clip = tutorialVideos[index];
        videoPlayer.Play();
        word.text = videoPlayer.clip.name;
    }

    private void NextVideo()
    {
        currVideoIndex++;
        if (currVideoIndex >= tutorialVideos.Length)
        {
            currVideoIndex = 0;
        }
        PlayVideo(currVideoIndex);
    }
    private void PreviousVideo()
    {
        currVideoIndex--;
        if (currVideoIndex < 0)
        {
            currVideoIndex = tutorialVideos.Length - 1;
        }
        PlayVideo(currVideoIndex);
    }
}


