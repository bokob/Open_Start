using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamHandler : MonoBehaviour
{
    // 카메라 화면을 표시할 게임 오브젝트
    // 유니티 Inspector에서 지정되어야 함
    // Renderer 컴퍼넌트를 포함해야 함
    public GameObject objectTarget = null;

    // 카메라 입력을 위한 WebCamTexture
    protected WebCamTexture textureWebCam = null;

    void Start()
    {
        // 현재 사용 가능한 카메라의 리스트
        WebCamDevice[] devices = WebCamTexture.devices;

        // 사용할 카메라 선택
        // 가장 처음 검색되는 후면 카메라 사용
        int selectedCameraIndex = -1;
        for(int i=0;i<devices.Length;i++)
        {
            // 사용 가능한 카메라 로그
            Debug.Log("Available Webcam: " + devices[i].name + ((devices[i].isFrontFacing) ? "(Front)" : "(Back)"));

            // 후면 카메라인지 체크
            if(devices[i].isFrontFacing == false)
            {
                // 해당 카메라 선택
                selectedCameraIndex = i;
                break;
            }
        }

        // WebCamTexture 생성
        if(selectedCameraIndex >= 0)
        {
            // 선택된 카메라에 대한 새로운 WebCamTexture 생성
            textureWebCam = new WebCamTexture(devices[selectedCameraIndex].name);

            // 원하는 FPS를 설정
            if (textureWebCam != null)
            {
                textureWebCam.requestedFPS = 60;
            }
        }

        // objectTarget으로 카메라가 표시되도록 설정
        if (textureWebCam != null)
        {
            // objectTarget에 포함된 Renderer
            Renderer render = objectTarget.GetComponent<Renderer>();

            // 해당 Renderer의 mainTexture를 WebCamTexture로 설정
            render.material.mainTexture = textureWebCam;
        }
    }

    void OnDestroy()
    {
        // WebCamTexture 리소스 반환
        if (textureWebCam != null)
        {
            textureWebCam.Stop();
            WebCamTexture.Destroy(textureWebCam);
            textureWebCam = null;
        }
    }

    // Play 버튼이 눌렸을 때
    // 유니티 Inspector에서 버튼 연결 필요
    public void OnPlayButtonClick()
    {
        // 카메라 구동 시작
        if (textureWebCam != null)
        {
            textureWebCam.Play();
        }
    }

    // Stop 버튼이 눌렸을 때
    // 주의! 유니티 Inspector에서 버튼 연결 필요
    public void OnStopButtonClick()
    {
        // 카메라 구동 정지
        if (textureWebCam != null)
        {
            textureWebCam.Stop();
        }
    }
}
