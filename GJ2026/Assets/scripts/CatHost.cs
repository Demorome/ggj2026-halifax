using Unity.VisualScripting;
using UnityEngine;

public class CatHost : Host
{
    [field: SerializeField] public override float maxHostHealthTimer { get; set; } = 5f;
    protected override float CurrentHostHealthTimer { get; set; }

    [field: SerializeField] public override float mass { get; set; } = 1f;
    [field: SerializeField] public override PlayerController playerController { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHostHealthTimer = maxHostHealthTimer;
    }

    // Update is called once per frame
    void Update()
    {
        var gameManager = GameManager.Instance;
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal
            || gameManager.IsLoading
            || IsEquipped)
        {
            return;
        }

        base.CountDownHostLife();
    }
}
