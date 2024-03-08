using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WebUtils;

public class Test : MonoBehaviour
{
    public bool _useExtension;

    public DownloadInfo singleImage;
    public DownloadInfoWithData singleImageWithData;
    public DownloadInfo[] downloads;
    public DownloadInfoWithData[] downloadsWithData;

    void OnDisable()
    {
        Cleanup();
    }

    void OnEnable()
    {
        if (_useExtension)
        {
            Debug.Log("Using Extensions");
            singleImage.Image.DownloadAndSet(singleImage.URL);
            singleImageWithData.Image.DownloadAndSet(singleImage.URL, singleImageWithData, SetText);
            foreach (var info in downloads)
            {
                info.Image.DownloadAndSet(singleImage.URL);
            }

            foreach (var info in downloadsWithData)
            {
                info.Image.DownloadAndSet(singleImage.URL, singleImageWithData, SetText);
            }

        }
        else
        {
            Debug.Log("Not Using Extensions");
            ImageDownloader.Download(singleImage.URL, singleImage.Callback);
            ImageDownloader.Download(singleImageWithData.URL, singleImageWithData.text, singleImageWithData.Callback);
            ImageDownloader.Download(NoDataEnum(), () => Debug.Log("Done All with data 1"));
            ImageDownloader.Download(DataEnum(), () => Debug.Log("Done All with data"));
        }
    }

    private void Cleanup()
    {
        singleImage.Image.sprite = null;
        singleImageWithData.Image.sprite = null;
        singleImageWithData.TextMesh.text = "";
        foreach (var info in downloads)
        {
            info.Image.sprite = null;
        }

        foreach (var info in downloadsWithData)
        {
            info.Image.sprite = null;
            info.TextMesh.text = "";
        }
    }

    private void SetText(Sprite sprite, DownloadInfoWithData data)
    {
        data.TextMesh.text = data.text;
    }

    private IEnumerable<DownloadArgs<string>> DataEnum()
    {
        return downloadsWithData.Select(data => new DownloadArgs<string>(data.URL, data.text, data.Callback));
    }

    public IEnumerable<DownloadArgs> NoDataEnum()
    {
        return downloads.Select(data => new DownloadArgs(data.URL, data.Callback));
    }


    [Serializable]
    public class DownloadInfo
    {
        public string URL;
        public Image Image;

        public void Callback(Sprite sprite)
        {
            Image.sprite = sprite;
        }
    }

    [Serializable]
    public class DownloadInfoWithData
    {
        public string URL;
        public Image Image;
        public Text TextMesh;
        public string text;


        public void Callback(Sprite sprite, string data)
        {
            Image.sprite = sprite;
            TextMesh.text = data;
        }
    }
}