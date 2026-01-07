using SoftwareConsultingPlatform.Core.Models.UserAggregate;
using SoftwareConsultingPlatform.Core.Models.UserAggregate.Enums;

namespace SoftwareConsultingPlatform.Core.Tests.Models;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeUser_WithCorrectValues()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var email = "test@test.com";
        var passwordHash = "hashedpassword";
        var fullName = "Test User";

        // Act
        var user = new User(tenantId, email, passwordHash, fullName);

        // Assert
        Assert.Equal(tenantId, user.TenantId);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(fullName, user.FullName);
        Assert.Equal(UserStatus.Pending, user.Status);
        Assert.False(user.EmailVerified);
        Assert.NotEqual(Guid.Empty, user.UserId);
    }

    [Fact]
    public void VerifyEmail_ShouldSetEmailVerifiedToTrue()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@test.com", "hash", "Test");
        user.SetEmailVerificationToken("token");

        // Act
        user.VerifyEmail();

        // Assert
        Assert.True(user.EmailVerified);
        Assert.Equal(UserStatus.Active, user.Status);
        Assert.Null(user.EmailVerificationToken);
    }

    [Fact]
    public void Lock_ShouldSetUserToLockedStatus()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@test.com", "hash", "Test");
        var duration = TimeSpan.FromMinutes(15);

        // Act
        user.Lock(duration);

        // Assert
        Assert.Equal(UserStatus.Locked, user.Status);
        Assert.NotNull(user.LockedUntil);
    }

    [Fact]
    public void EnableMfa_ShouldSetMfaEnabledToTrue()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "test@test.com", "hash", "Test");
        var mfaSecret = "SECRET";

        // Act
        user.EnableMfa(mfaSecret);

        // Assert
        Assert.True(user.MfaEnabled);
        Assert.Equal(mfaSecret, user.MfaSecret);
    }
}
