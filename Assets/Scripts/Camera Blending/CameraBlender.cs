using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineBlendListCamera;

// The CameraBlender abstracts Cinemachine's blending logic for our specific purposes.
// It is intended to work in conjunction with FocusAreas.
public class CameraBlender : MonoBehaviour
{
    // Blend list camera component
    CinemachineBlendListCamera blendListCam;

    // Virtual cameras to blend between

    CinemachineVirtualCamera trackingCam;
    CinemachineVirtualCamera focusCam;

    // Blending instructions
    Instruction trackingInstruction = new Instruction();
    Instruction focusInstruction = new Instruction();


    // Start is called before the first frame update
    void Start()
    {
        blendListCam = GetComponent<CinemachineBlendListCamera>();

        // Get the cameras
        trackingCam = (CinemachineVirtualCamera)(blendListCam.ChildCameras[0]);
        focusCam = (CinemachineVirtualCamera)(blendListCam.ChildCameras[1]);

        // Create the instructions
        trackingInstruction.m_Blend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Style.EaseInOut,
            1.0f
        );
        trackingInstruction.m_Hold = 0f;
        trackingInstruction.m_VirtualCamera = trackingCam;

        focusInstruction.m_Blend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Style.EaseInOut,
            1.0f
        );
        focusInstruction.m_Hold = 0f;
        focusInstruction.m_VirtualCamera = focusCam;

        // Initialize starting instructions: use the tracking camera.
        blendListCam.m_Instructions = new Instruction[1];
        blendListCam.m_Instructions[0] = trackingInstruction;
    }

    public void UpdateFocusCamera(Vector2 pos, float size)
    {
        focusCam.transform.position = new Vector3(
            pos.x,
            pos.y,
            trackingCam.transform.position.z);  // We don't want to move the focus camera in the z axis
        focusCam.m_Lens.OrthographicSize = size;
    }

    public void Focus()
    {
        // Clear instructions list
        blendListCam.m_Instructions = new Instruction[2];

        // Add instructions to list
        blendListCam.m_Instructions[0] = trackingInstruction;
        blendListCam.m_Instructions[1] = focusInstruction;
    }

    public void Unfocus()
    {
        // Clear instructions list
        blendListCam.m_Instructions = new Instruction[2];

        // Add the cameras back
        blendListCam.m_Instructions[0] = focusInstruction;
        blendListCam.m_Instructions[1] = trackingInstruction;
    }
}
