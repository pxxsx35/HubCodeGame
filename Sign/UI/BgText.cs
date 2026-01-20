using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgText : MonoBehaviour
{
    public Text textItem;
    public Image bgText;

    // Update is called once per frame
    void Update()
    {
        if (textItem != null)
        {
            if (textItem.text == "")
            {
                bgText.gameObject.SetActive(false);
            }
            else
            {
                bgText.gameObject.SetActive(true);

            }
        }
    }
}
