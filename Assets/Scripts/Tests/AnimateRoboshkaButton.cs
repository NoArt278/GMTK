using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimateRoboshkaButton : MonoBehaviour
{
    [SerializeField] private Animator[] animator;
    [SerializeField] private TextMeshProUGUI buttonText;

    private bool opened = false;

    public void ToggleOpenMouth() {
        if (opened) {
            foreach (var anim in animator) {
                anim.SetBool("OpenMouth", false);
            }

            buttonText.text = "Open";
        } else {
            foreach (var anim in animator) {
                anim.SetBool("OpenMouth", true);
            }
            buttonText.text = "Close";
        }
        opened = !opened;
    }
}
