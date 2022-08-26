namespace FetchBitlocker.Agent;

public static class TestSendAgent
{
    public static async Task Main()
    {
        await HandleData.SendModelData(HandleData.GetBitlockerState());
    }
}
