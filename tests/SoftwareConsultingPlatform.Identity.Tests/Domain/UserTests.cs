using FluentAssertions;
using SoftwareConsultingPlatform.Identity.Core.Aggregates;
using Xunit;

namespace SoftwareConsultingPlatform.Identity.Tests.Domain;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var email = "test@example.com";
        var fullName = "Test User";
        var passwordHash = "hashedpassword123";

        // Act
        var user = new User(tenantId, email, fullName, passwordHash);

        // Assert
        user.UserId.Should().NotBeEmpty();
        user.TenantId.Should().Be(tenantId);
        user.Email.Should().Be(email);
        user.FullName.Should().Be(fullName);
        user.Status.Should().Be("Active");
        user.EmailVerified.Should().BeFalse();
        user.MfaEnabled.Should().BeFalse();
    }

    [Fact]
    public void VerifyEmail_ShouldSetEmailVerifiedToTrue()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        user.VerifyEmail();

        // Assert
        user.EmailVerified.Should().BeTrue();
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLoginAndResetFailedAttempts()
    {
        // Arrange
        var user = CreateTestUser();
        var ipAddress = "192.168.1.1";
        var deviceInfo = "Chrome on Windows";

        // Act
        user.RecordLogin(ipAddress, deviceInfo);

        // Assert
        user.LastLoginAt.Should().NotBeNull();
        user.LastLoginIp.Should().Be(ipAddress);
        user.FailedLoginAttempts.Should().Be(0);
    }

    [Fact]
    public void Lock_ShouldSetStatusToLockedAndSetLockoutEnd()
    {
        // Arrange
        var user = CreateTestUser();
        var lockoutEnd = DateTime.UtcNow.AddHours(1);

        // Act
        user.Lock(lockoutEnd);

        // Assert
        user.Status.Should().Be("Locked");
        user.LockoutEnd.Should().Be(lockoutEnd);
    }

    [Fact]
    public void EnableMfa_ShouldSetMfaEnabledToTrue()
    {
        // Arrange
        var user = CreateTestUser();
        var secret = "JBSWY3DPEHPK3PXP";

        // Act
        user.EnableMfa(secret);

        // Assert
        user.MfaEnabled.Should().BeTrue();
        user.MfaSecret.Should().Be(secret);
    }

    [Fact]
    public void DisableMfa_ShouldSetMfaEnabledToFalse()
    {
        // Arrange
        var user = CreateTestUser();
        user.EnableMfa("JBSWY3DPEHPK3PXP");

        // Act
        user.DisableMfa();

        // Assert
        user.MfaEnabled.Should().BeFalse();
        user.MfaSecret.Should().BeNull();
    }

    [Fact]
    public void UpdatePassword_ShouldUpdatePasswordHashAndTimestamp()
    {
        // Arrange
        var user = CreateTestUser();
        var newPasswordHash = "newhashedpassword456";

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.PasswordChangedAt.Should().NotBeNull();
    }

    private static User CreateTestUser()
    {
        return new User(Guid.NewGuid(), "test@example.com", "Test User", "hashedpassword123");
    }
}
