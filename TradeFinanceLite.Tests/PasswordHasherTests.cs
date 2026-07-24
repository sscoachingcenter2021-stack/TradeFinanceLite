using Xunit;

public class PasswordHasherTests
{
	[Fact]
	public void Hash_And_Verify_CorrectPassword_ReturnsTrue()
	{
		// Arrange
		string password = "MySecureP@ss123";

		// Act
		string hash = PasswordHasher.Hash(password);
		bool result = PasswordHasher.Verify(password, hash);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void Verify_WrongPassword_ReturnsFalse()
	{
		// Arrange
		string password = "MySecureP@ss123";
		string wrongPassword = "WrongPassword456";

		// Act
		string hash = PasswordHasher.Hash(password);
		bool result = PasswordHasher.Verify(wrongPassword, hash);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void Hash_SamePassword_ProducesDifferentHashes()
	{
		// Arrange
		string password = "MySecureP@ss123";

		// Act
		string hash1 = PasswordHasher.Hash(password);
		string hash2 = PasswordHasher.Hash(password);

		// Assert — different salts should produce different hashes
		Assert.NotEqual(hash1, hash2);
	}
}