using System.Linq;
using Niantic.Lightship.AR;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Niantic.Lightship.AR.ARFoundation;


public class SemanticQuerying : MonoBehaviour
{
    public ARCameraManager _cameraMan;
    public ARSemanticSegmentationManager _semanticMan;

    public RawImage _image;
    public Material _material;

    private string _semanticChannelName = string.Empty;

    //private string _channel = "ground";

    [HideInInspector]
    [SerializeField]
    private Dropdown _channelDropdown;

    void OnEnable()
    {
        _cameraMan.frameReceived += OnCameraFrameUpdate;
        _channelDropdown.onValueChanged.AddListener(ChannelDropdown_OnValueChanged);

        // Subscribe to the SemanticModelIsReady event
        _semanticMan.SemanticModelIsReady += SemanticsManager_OnModelReady;
    }

    private void OnDisable()
    {
        _cameraMan.frameReceived -= OnCameraFrameUpdate;

        // Unsubscribe from the SemanticModelIsReady event
        _semanticMan.SemanticModelIsReady -= SemanticsManager_OnModelReady;
    }

    private void OnCameraFrameUpdate(ARCameraFrameEventArgs args)
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

        //get the semantic texture
        Matrix4x4 mat = Matrix4x4.identity;
        var texture = _semanticMan.GetSemanticChannelTexture(_semanticChannelName, out mat);

        if (texture)
        {
            //the texture needs to be aligned to the screen so get the display matrix
            //and use a shader that will rotate/scale things.
            Matrix4x4 cameraMatrix = args.displayMatrix ?? Matrix4x4.identity;
            _image.material = _material;
            _image.material.SetTexture("_SemanticTex", texture);
            _image.material.SetMatrix("_DisplayMatrix", mat);
        }
    }

    private void SemanticsManager_OnModelReady(ARSemanticModelReadyEventArgs args)
    {
        // Initialize the channel names in the dropdown menu.
    var channelNames = args.SemanticChannelNames;
        _channelDropdown.AddOptions(channelNames.ToList());


        // Display artificial ground by default.
        _semanticChannelName = channelNames[3];
        var dropdownList = _channelDropdown.options.Select(option => option.text).ToList();
        _channelDropdown.value = dropdownList.IndexOf(_semanticChannelName);
    }
    private void ChannelDropdown_OnValueChanged(int val)
    {
        // Update the display channel from the dropdown value.
        _semanticChannelName = _channelDropdown.options[val].text;
    }
}