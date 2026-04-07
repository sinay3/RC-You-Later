using UnityEngine;
using TMPro;

public class CheckpointPreview : MonoBehaviour
{
    public TMP_Text previewText;

    void Start()
    {
        // S’assurer que le timer a chargé les steps avant d’afficher
        Timer.Load();

        UpdatePreview();
    }

    public void UpdatePreview()
    {
        if (Timer.StepsCount == 0)
        {
            previewText.text = "Aucun ancien split";
            return;
        }

        string text = "Ancien splits :\n";

        for (int i = 0; i < Timer.StepsCount; i++)
        {
            double seconds = Timer.GetStepElapsedSeconds(i);
            text += $"Checkpoint {i + 1} : {seconds:F2}s\n";
        }

        previewText.text = text;
    }
}