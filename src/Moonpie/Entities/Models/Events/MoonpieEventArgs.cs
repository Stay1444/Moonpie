namespace Moonpie.Entities.Models.Events;

public abstract class MoonpieEventArgs
{
    public bool Handled { get; set; }

    internal bool Cancelled { get; set; }
    public void Cancel()
    {
        this.Cancelled = true;
        this.Handled = true;
    }
}