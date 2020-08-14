using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class ShockwaveTrigger : MonoBehaviour
{

    [Tooltip("Shockwave Distortion Material")]
    [Space]
    [SerializeField] private Material _shockwaveMaterial;

    [Space]
    [Header("Shockwave Animation Params")]

    [Tooltip("This is the total size Shockwave will expand on the screen from the center point. Set the value to 1 to cover the entire screen space.")]    
    [SerializeField] private float _shockwaveExpansion;
    [Tooltip("The speed at which the Shockwave will expand on the screen.")]
    [SerializeField] private float _expansionSpeed;
    [Tooltip("Check this on to create a reverse Shockwave effect. The Shockwave will shrink from the expansion size to 0 at the center point.")]
    [SerializeField] private bool _invertExpansion;
    
    [HideInInspector]
    public bool animateWaveParams;
    [HideInInspector]
    public float startDistortionIntensity;
    [HideInInspector]
    public float endDistortionIntensity;
    [HideInInspector]
    public float startThickness;
    [HideInInspector]
    public float endThickness;

    private bool _instantiateShockwave;
    private float size = 0;
    private float force = 0;
    private float thickness = 0;
    private float forceTimeParam;
    private float thickTimeParam;

    public Material GetShockwaveMaterial{
        get{ return _shockwaveMaterial;}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Fire1")) {
            InstantiateShockwave();
        }

        if(!_invertExpansion)
            StandardShockwave();
        else
            ReverseShockwave();

        AnimateShockwaveParams();
    }

    private void InstantiateShockwave(){
        forceTimeParam = 0;
        thickTimeParam = 0;

        force = startDistortionIntensity;
        thickness = startThickness;

        _instantiateShockwave = true;

        if(_invertExpansion){
            size = _shockwaveExpansion;
        }
    }

    private void StandardShockwave(){

        if(_instantiateShockwave){

            if(size >= _shockwaveExpansion)
            {
                size = 0;
                _shockwaveMaterial.SetFloat("_Size", size);
                _instantiateShockwave = false;
            }
            else
            {
                size += Time.deltaTime * _expansionSpeed;
                _shockwaveMaterial.SetFloat("_Size", size);
            }
        }
    }

    private void ReverseShockwave(){

        if(_instantiateShockwave){

            if(size > 0)
            {
                size -= Time.deltaTime * _expansionSpeed;
                _shockwaveMaterial.SetFloat("_Size", size);
            }
            else
            {
                size = 0;
                _shockwaveMaterial.SetFloat("_Size", size);
                _instantiateShockwave = false;
            }
        }
    }

    private void AnimateShockwaveParams(){

        if(animateWaveParams)
        {
            //Animate Intensity
            if(endDistortionIntensity < force){
                forceTimeParam -= (Time.deltaTime * _expansionSpeed) / _shockwaveExpansion;
                force = Mathf.Lerp(endDistortionIntensity, startDistortionIntensity, forceTimeParam);
                _shockwaveMaterial.SetFloat("_Force", force);
                
                if(force <= endDistortionIntensity)
                    force = endDistortionIntensity;
            }
            else{
                forceTimeParam += (Time.deltaTime * _expansionSpeed) / _shockwaveExpansion;
                force = Mathf.Lerp(startDistortionIntensity, endDistortionIntensity, forceTimeParam);
                _shockwaveMaterial.SetFloat("_Force", force);

                if(force >= endDistortionIntensity)
                    force = endDistortionIntensity;
            }

            //Animate Thickness
            if(endThickness < thickness){
                thickTimeParam -= (Time.deltaTime * _expansionSpeed) / _shockwaveExpansion;
                thickness = Mathf.Lerp(endThickness, startThickness, thickTimeParam);
                _shockwaveMaterial.SetFloat("_Thickness", thickness);

                if(thickness <= endThickness)
                    thickness = endThickness;
            }
            else{
                thickTimeParam += (Time.deltaTime * _expansionSpeed) / _shockwaveExpansion;
                thickness = Mathf.Lerp(startThickness, endThickness, thickTimeParam);
                _shockwaveMaterial.SetFloat("_Thickness", thickness);

                if(thickness >= endThickness)
                    thickness = endThickness;
            }
        }
    }

    /* public float hSliderValue = 0.0F;
    void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(0, 0, 100, 100), hSliderValue, 0.0F, 10.0F);
    } */
    /* Vector2 screenPos = new Vector2 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            _shockwaveMaterial.SetVector ("_Center", screenPos);
            StopAllCoroutines();
            StartCoroutine(ShockWaveEffect()); */

    /* IEnumerator ShockWaveEffect () {
        float tParam = 0;
        float force;
        float size;
        while (tParam < 1) {
            tParam += Time.deltaTime * 2;
            force = Mathf.Lerp(0.02f, 0.1f, tParam);
            size = Mathf.Lerp(0f, 1f, tParam);
            _shockwaveMaterial.SetFloat("_Force", force);
            _shockwaveMaterial.SetFloat("_Size", size);
            yield return null;
        }
    } */
}

#if UNITY_EDITOR

    [CustomEditor(typeof(ShockwaveTrigger))]
    public class ShockwaveTriggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ShockwaveTrigger script = (ShockwaveTrigger)target;

            EditorGUILayout.Space();
            script.animateWaveParams = EditorGUILayout.Toggle(new GUIContent("Animate Wave Params", "Check this on to animate Shockwave parameters like 'Distortion Intensity' and 'Thickness' during the expansion animation."), script.animateWaveParams);
            
            if(script.animateWaveParams)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Start Distortion Intensity: " + script.startDistortionIntensity);
                EditorGUILayout.LabelField("End Distortion Intensity: " + script.endDistortionIntensity);
                EditorGUILayout.LabelField("Start Thickness: " + script.startThickness);
                EditorGUILayout.LabelField("End Thickness: " + script.endThickness);
                EditorGUILayout.Space();

                GUI.skin.button.wordWrap = true;
                GUILayout.BeginHorizontal();
                if(GUILayout.Button(new GUIContent("Set Start Distortion", "Use this button to set the actual Shockwave distortion as the start value."), 
                                GUILayout.Width(120), GUILayout.Height(40)))
                {
                    script.startDistortionIntensity = script.GetShockwaveMaterial.GetFloat("_Force");
                }
                GUILayout.FlexibleSpace();
                if(GUILayout.Button(new GUIContent("Set End Distortion", "Use this button to set the actual Shockwave distortion as the end value."), 
                                GUILayout.Width(120), GUILayout.Height(40)))
                {
                    script.endDistortionIntensity = script.GetShockwaveMaterial.GetFloat("_Force");
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if(GUILayout.Button(new GUIContent("Set Start Thickness", "Use this button to set the actual Shockwave distortion as the start value."), 
                                GUILayout.Width(120), GUILayout.Height(40)))
                {
                    script.startThickness = script.GetShockwaveMaterial.GetFloat("_Thickness");
                }
                GUILayout.FlexibleSpace();
                if(GUILayout.Button(new GUIContent("Set End Thickness", "Use this button to set the actual Shockwave distortion as the end value."), 
                                GUILayout.Width(120), GUILayout.Height(40)))
                {
                    script.endThickness = script.GetShockwaveMaterial.GetFloat("_Thickness");
                }
                GUILayout.EndHorizontal();
            }

        }
    }
#endif
