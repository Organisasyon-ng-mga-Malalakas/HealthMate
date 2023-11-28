namespace HealthMate.Interfaces;
public interface IBiometricService
{
	Task<bool> AuthenticateAsync(string title, string reason, string description);
	bool IsAvailable();
}
