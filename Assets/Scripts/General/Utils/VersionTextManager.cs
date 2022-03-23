using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;

    private void Awake() {
        labelText.text = Application.version;
    }
}
