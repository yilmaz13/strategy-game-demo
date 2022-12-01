using Builds;

public class PowerPlants : Build
{
    public override void OpenInformationPopup()
    {
        UIManager.Instance.CloseInformationPopup();
    }
}