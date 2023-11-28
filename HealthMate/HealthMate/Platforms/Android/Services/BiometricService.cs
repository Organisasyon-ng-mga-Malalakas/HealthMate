using AndroidX.Biometric;
using HealthMate.Interfaces;
using Java.Lang;
using static AndroidX.Biometric.BiometricPrompt;
using Application = Android.App.Application;

namespace HealthMate.Platforms.Android.Services;
public class BiometricService : IBiometricService
{
	public Task<bool> AuthenticateAsync(string title, string subtitle, string description)
	{
		var taskCompletionSource = new TaskCompletionSource<bool>();
		var promptInfo = new PromptInfo.Builder()
			.SetAllowedAuthenticators(BiometricManager.Authenticators.BiometricStrong | BiometricManager.Authenticators.DeviceCredential)
			.SetSubtitle(subtitle)
			.SetConfirmationRequired(false)
			.SetDescription(description)
			.SetTitle(title)
			.Build();

		var context = Platform.CurrentActivity as MainActivity;
		var biometricPrompt = new BiometricPrompt(context, new AuthenticationCallback(taskCompletionSource));
		biometricPrompt.Authenticate(promptInfo);

		return taskCompletionSource.Task;
	}

	public bool IsAvailable()
	{
		var biometricManager = BiometricManager.From(Application.Context);
		var isAvailable = biometricManager.CanAuthenticate(BiometricManager.Authenticators.BiometricStrong | BiometricManager.Authenticators.DeviceCredential) == BiometricManager.BiometricSuccess;

		return isAvailable;
	}
}

public class AuthenticationCallback(TaskCompletionSource<bool> taskCompletionSource) : BiometricPrompt.AuthenticationCallback
{
	public override void OnAuthenticationSucceeded(AuthenticationResult result)
	{
		base.OnAuthenticationSucceeded(result);
		taskCompletionSource.TrySetResult(true);
	}

	public override void OnAuthenticationFailed()
	{
		base.OnAuthenticationFailed();
		taskCompletionSource.TrySetResult(false);
	}

	public override void OnAuthenticationError(int errorCode, ICharSequence errString)
	{
		base.OnAuthenticationError(errorCode, errString);
		taskCompletionSource.TrySetResult(false);
	}
}
