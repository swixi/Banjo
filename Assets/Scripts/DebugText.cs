using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public Text debugText;
    public BallManager ball;

    void Update()
    {
        debugText.text = "fps: " + (1.0f / Time.smoothDeltaTime).ToString("0.0");
        debugText.text += "\n";
        debugText.text += "attached player: " + ball.IsAttached();
    }
}
