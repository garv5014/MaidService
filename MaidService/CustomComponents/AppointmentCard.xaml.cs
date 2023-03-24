using MaidService.ComponentsViewModels;

namespace MaidService.CustomComponents;

public partial class AppointmentCard : Frame
{
    public AppointmentCardViewModel VM { get; private set; }

    public AppointmentCard()
	{
		InitializeComponent();
		VM = new AppointmentCardViewModel();
	}
}