using HealthMate.Templates;
using HealthMate.ViewModels.Questions;

namespace HealthMate.Views.Questions;

public partial class QuestionPage : BasePage<QuestionsPageViewModel>
{
	public QuestionPage(QuestionsPageViewModel vm) : base(vm)
	{
		InitializeComponent();
		//vm.Questions = new ObservableCollection<CategoryAndQuestion>(questions);
	}
}