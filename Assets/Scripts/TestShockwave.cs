using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShockwave : MonoBehaviour
{
    public Material _shockwaveMaterial;
    public CameraShake cameraShake;
    private float _shockwaveExpansion;
    private float _expansionSpeed;
    private float _size;
    private float _intensity;
    private float _thickness;
    private float _tempSize;
    private bool _instantiateShockwave;

    // Update is called once per frame
    void Update()
    {
        StandardShockwave();
        ChangeShaderValues();
        if(Input.GetKeyDown(KeyCode.T))
            Time.timeScale = 0.2f;
        else if(Input.GetKeyDown(KeyCode.Y))
            Time.timeScale = 1f;
    }

    private void InstantiateShockwave(){
        _tempSize = _size;
        _size = 0;
        _instantiateShockwave = true;
        cameraShake.startShaking = true;
    }

    private void StandardShockwave(){

        if(_instantiateShockwave){

            if(_size >= _shockwaveExpansion)
            {
                _size = _tempSize;
                _shockwaveMaterial.SetFloat("_Size", _size);
                _instantiateShockwave = false;
            }
            else
            {
                _size += Time.deltaTime * _expansionSpeed;
                _shockwaveMaterial.SetFloat("_Size", _size);
            }
        }
    }

    private void ChangeShaderValues(){
        if(!_instantiateShockwave){
            _shockwaveMaterial.SetFloat("_Size", _size);
            _shockwaveMaterial.SetFloat("_Force", _intensity);
            _shockwaveMaterial.SetFloat("_Thickness", _thickness);
        }
    }

    void OnGUI(){

        GUI.Box(new Rect(0, 0, 180, 130), "Visualization Params");
        GUI.Box(new Rect(0, 130, 180, 150), "Animation Params");

        GUI.Label(new Rect(10, 25, 150, 20), "Size: " + Mathf.Round(_size * 100f) / 100f);
        _size = GUI.HorizontalSlider(new Rect(10, 45, 80, 20), _size, 0.0f, 1.5f);
        GUI.Label(new Rect(10, 55, 150, 20), "Distortion Intensity: " + Mathf.Round(_intensity * 100f) / 100f);
        _intensity = GUI.HorizontalSlider(new Rect(10, 75, 80, 20), _intensity, 0.0f, 0.5f);
        GUI.Label(new Rect(10, 85, 150, 20), "Thickness: " + Mathf.Round(_thickness * 100f) / 100f);
        _thickness = GUI.HorizontalSlider(new Rect(10, 105, 80, 20), _thickness, 0.0f, 0.5f);

        /* _intensity = GUI.HorizontalSlider(new Rect(10, 20, 100, 20), _intensity, 0.0f, 10f);
        _thickness = GUI.HorizontalSlider(new Rect(10, 30, 100, 20), _thickness, 0.0f, 10f); */

        GUI.Label(new Rect(10, 155, 200, 20), "Animation Size: " + Mathf.Round(_shockwaveExpansion * 100f) / 100f);
        _shockwaveExpansion = GUI.HorizontalSlider(new Rect(10, 175, 80, 20), _shockwaveExpansion, 0.0f, 1.5f);
        GUI.Label(new Rect(10, 185, 200, 20), "Animation Speed: " + Mathf.Round(_expansionSpeed * 100f) / 100f);
        _expansionSpeed = GUI.HorizontalSlider(new Rect(10, 210, 80, 20), _expansionSpeed, 0.0f, 5f);
        if (GUI.Button(new Rect(10, 235, 80, 35), "Play Effect")){
            InstantiateShockwave();
        }
    }
}
