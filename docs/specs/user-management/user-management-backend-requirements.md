# User Management - Backend Requirements

## Overview
The backend user management system provides APIs for creating, reading, updating, and deleting users, managing roles and permissions, and tracking user activity.

## FR-USER-BE-001: User Data Model
**Requirement Statement**: The system shall define a comprehensive user data model for storing user information and authentication data.

**Acceptance Criteria**:
- AC1: User entity includes fields: id, tenantId, email, passwordHash, fullName, phone, companyName
- AC2: User includes security fields: emailVerified, emailVerificationToken, passwordResetToken, mfaSecret, mfaEnabled
- AC3: User includes status fields: status (active/inactive/pending/locked), lockedUntil
- AC4: User includes metadata fields: avatarUrl, lastLoginAt, loginCount, createdAt, updatedAt, deletedAt
- AC5: User includes relationships: roles (many-to-many), sessions (one-to-many), activityLogs (one-to-many)
- AC6: Email is unique within tenant (composite unique index on tenantId + email)
- AC7: Password hash uses bcrypt or Argon2 with minimum 10 rounds
- AC8: Soft delete is supported via deletedAt timestamp

## FR-USER-BE-002: User CRUD APIs
**Requirement Statement**: The system shall provide RESTful APIs for creating, reading, updating, and deleting users.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users creates a new user
- AC2: API endpoint GET /api/users returns paginated list of users
- AC3: API endpoint GET /api/users/{id} retrieves specific user details
- AC4: API endpoint PUT /api/users/{id} updates user information
- AC5: API endpoint DELETE /api/users/{id} soft-deletes user
- AC6: All endpoints enforce tenant isolation (users belong to tenant)
- AC7: Create and update endpoints validate all input data
- AC8: Endpoints require appropriate role authorization (UserAdmin, TenantAdmin)

## FR-USER-BE-003: User Listing and Pagination
**Requirement Statement**: The system shall provide API for listing users with pagination, filtering, and sorting.

**Acceptance Criteria**:
- AC1: API endpoint GET /api/users supports page and pageSize parameters
- AC2: Default page size is 25, maximum page size is 100
- AC3: Response includes pagination metadata (totalCount, totalPages, currentPage)
- AC4: API supports filtering by role, status, and email verification status
- AC5: API supports search by name, email, or company
- AC6: API supports sorting by name, email, createdAt, lastLoginAt
- AC7: API response time is under 200ms for typical queries
- AC8: Database queries use appropriate indexes for performance

## FR-USER-BE-004: User Search
**Requirement Statement**: The system shall provide full-text search capability for finding users.

**Acceptance Criteria**:
- AC1: API endpoint GET /api/users/search accepts query parameter 'q'
- AC2: Search matches against name, email, and company fields
- AC3: Search implements case-insensitive partial matching
- AC4: Search results are ranked by relevance
- AC5: Search is tenant-scoped (only searches within tenant)
- AC6: Search response includes highlighted matched fields
- AC7: Search response time is under 300ms
- AC8: Search implements appropriate database indexes or search engine

## FR-USER-BE-005: User Creation
**Requirement Statement**: The system shall provide API for creating new user accounts with validation and notification.

**Acceptance Criteria**:
- AC1: API validates required fields: email, fullName, tenantId
- AC2: API validates email format and uniqueness within tenant
- AC3: API validates password complexity if password is provided
- AC4: API generates secure random password if not provided
- AC5: API creates user with pending status if email verification is required
- AC6: API sends welcome/verification email to new user
- AC7: API assigns default "User" role if no role specified
- AC8: API returns 201 Created with user ID and sends location header
- AC9: API implements transaction to ensure atomic user creation

## FR-USER-BE-006: User Update
**Requirement Statement**: The system shall provide API for updating existing user information with validation.

**Acceptance Criteria**:
- AC1: API allows updating: fullName, phone, companyName, status, roles
- AC2: Email updates require re-verification workflow
- AC3: Password updates require separate endpoint (security)
- AC4: API validates all updates before persistence
- AC5: API returns 200 OK with updated user data
- AC6: API logs all user updates in activity log
- AC7: Users can update their own profile (limited fields)
- AC8: Admins can update any user within their tenant

## FR-USER-BE-007: User Deletion
**Requirement Statement**: The system shall provide API for soft-deleting users with proper cleanup.

**Acceptance Criteria**:
- AC1: API implements soft delete (sets deletedAt timestamp)
- AC2: Soft-deleted users are excluded from normal queries
- AC3: Soft-deleted users cannot log in
- AC4: API endpoint POST /api/users/{id}/restore restores soft-deleted user
- AC5: Hard delete (permanent) is available via separate admin endpoint
- AC6: Hard delete removes all user data and associations
- AC7: Deletion requires confirmation and authorization
- AC8: Deletion is logged in audit trail

## FR-USER-BE-008: Role Assignment API
**Requirement Statement**: The system shall provide APIs for assigning and removing roles from users.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users/{id}/roles/{roleId} assigns role to user
- AC2: API endpoint DELETE /api/users/{id}/roles/{roleId} removes role from user
- AC3: API endpoint GET /api/users/{id}/roles returns all roles for user
- AC4: Role assignments are tenant-scoped
- AC5: API validates role exists before assignment
- AC6: API prevents duplicate role assignments
- AC7: API requires UserAdmin or TenantAdmin authorization
- AC8: Role changes are logged in activity log

## FR-USER-BE-009: User Activity Logging
**Requirement Statement**: The system shall track and store user activity for audit and security purposes.

**Acceptance Criteria**:
- AC1: Activity log table stores: userId, tenantId, action, resource, ipAddress, userAgent, timestamp
- AC2: Key actions logged: login, logout, profile update, password change, role change
- AC3: API endpoint GET /api/users/{id}/activity returns activity log
- AC4: Activity log supports pagination and filtering by date range and action type
- AC5: Activity logs are immutable (insert-only)
- AC6: Activity logs are retained for 90 days
- AC7: Activity logging is asynchronous to avoid blocking requests
- AC8: Failed operations are also logged with error details

## FR-USER-BE-010: User Session Management
**Requirement Statement**: The system shall track and manage user sessions across devices.

**Acceptance Criteria**:
- AC1: Session table stores: userId, tenantId, refreshToken, deviceInfo, ipAddress, createdAt, lastUsedAt
- AC2: API endpoint GET /api/users/{id}/sessions returns active sessions
- AC3: API endpoint DELETE /api/users/{id}/sessions/{sessionId} revokes session
- AC4: API endpoint DELETE /api/users/{id}/sessions revokes all sessions
- AC5: Session list includes device type, browser, location derived from IP
- AC6: Sessions inactive for 30 days are automatically revoked
- AC7: Users can manage their own sessions
- AC8: Admins can revoke sessions for any user in tenant

## FR-USER-BE-011: User Invitation API
**Requirement Statement**: The system shall provide API for inviting new users via email.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users/invite accepts email and role
- AC2: API creates user record with pending invitation status
- AC3: API generates time-limited invitation token (valid 7 days)
- AC4: API sends invitation email with setup link
- AC5: API endpoint GET /api/users/invitations returns pending invitations
- AC6: API endpoint POST /api/users/invitations/{id}/resend resends invitation
- AC7: API endpoint DELETE /api/users/invitations/{id} cancels invitation
- AC8: Invitation acceptance creates active user account

## FR-USER-BE-012: User Avatar Management
**Requirement Statement**: The system shall provide APIs for uploading and managing user avatars.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users/{id}/avatar uploads avatar image
- AC2: API validates image format (JPEG, PNG) and size (max 5MB)
- AC3: API stores avatar in cloud storage with tenant-specific path
- AC4: API generates thumbnail versions (48x48, 96x96, 192x192)
- AC5: API updates user record with avatar URL
- AC6: API endpoint DELETE /api/users/{id}/avatar removes avatar
- AC7: Avatar URLs include CDN endpoint for performance
- AC8: Old avatars are deleted when new ones are uploaded

## FR-USER-BE-013: Bulk User Operations
**Requirement Statement**: The system shall provide APIs for performing bulk operations on multiple users.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users/bulk/update accepts array of user IDs and update payload
- AC2: API endpoint POST /api/users/bulk/delete soft-deletes multiple users
- AC3: API endpoint POST /api/users/bulk/assign-role assigns role to multiple users
- AC4: Bulk operations are executed in transaction for atomicity
- AC5: API returns success/failure count and details for each user
- AC6: Bulk operations are limited to 100 users per request
- AC7: Long-running bulk operations return 202 Accepted with job ID
- AC8: Job status can be queried via separate endpoint

## FR-USER-BE-014: User Import API
**Requirement Statement**: The system shall provide API for importing users from CSV file.

**Acceptance Criteria**:
- AC1: API endpoint POST /api/users/import accepts CSV file upload
- AC2: API validates CSV format and required columns (email, fullName, role)
- AC3: API validates each row for data integrity
- AC4: API returns validation errors with row numbers before import
- AC5: API executes import in background job for large files
- AC6: API sends email notification when import completes
- AC7: Import creates users with pending status requiring email verification
- AC8: Import summary includes success/failure count and error details

## FR-USER-BE-015: User Export API
**Requirement Statement**: The system shall provide API for exporting user data to CSV format.

**Acceptance Criteria**:
- AC1: API endpoint GET /api/users/export generates CSV export
- AC2: Export includes: name, email, role, status, created date, last login
- AC3: Export respects current filters and search parameters
- AC4: Export is tenant-scoped (only exports tenant users)
- AC5: Export is limited to 10,000 users per request
- AC6: Large exports are processed asynchronously with download link
- AC7: Export file is stored temporarily (24 hours) in cloud storage
- AC8: Export includes appropriate CSV headers

## FR-USER-BE-016: User Statistics API
**Requirement Statement**: The system shall provide API for retrieving user statistics and metrics.

**Acceptance Criteria**:
- AC1: API endpoint GET /api/users/stats returns user statistics
- AC2: Statistics include: total users, active users, inactive users, users by role
- AC3: Statistics include: new users this month, login activity trends
- AC4: Statistics are aggregated per tenant
- AC5: API supports date range filtering for time-based metrics
- AC6: Statistics are cached with 1-hour TTL
- AC7: API response time is under 200ms with caching
- AC8: Only admins can access user statistics

## FR-USER-BE-017: User Validation
**Requirement Statement**: The system shall implement comprehensive validation for user data.

**Acceptance Criteria**:
- AC1: Email validation: format check, uniqueness within tenant
- AC2: Password validation: minimum 8 characters, uppercase, lowercase, number, special character
- AC3: Name validation: minimum 2 characters, maximum 100 characters
- AC4: Phone validation: optional, valid phone number format
- AC5: Company name validation: maximum 200 characters
- AC6: Avatar validation: image format, size limits
- AC7: All validation errors return descriptive messages
- AC8: Validation is performed both at API layer and database constraints

## FR-USER-BE-018: Multi-Tenancy Enforcement
**Requirement Statement**: The system shall enforce tenant isolation for all user operations.

**Acceptance Criteria**:
- AC1: All user records include tenantId foreign key
- AC2: All API queries include tenant filter in WHERE clause
- AC3: Users cannot be queried or modified across tenants
- AC4: JWT tokens include tenantId claim validated on each request
- AC5: Database indexes include tenantId for query performance
- AC6: Row-level security policies enforce tenant isolation
- AC7: Cross-tenant user access attempts are logged as security events
- AC8: SuperAdmin role can access users across tenants

## FR-USER-BE-019: API Security and Authorization
**Requirement Statement**: The system shall implement security measures for user management APIs.

**Acceptance Criteria**:
- AC1: All endpoints use HTTPS/TLS 1.2 or higher
- AC2: All endpoints require JWT authentication except public registration
- AC3: User CRUD endpoints require UserAdmin or TenantAdmin role
- AC4: Users can only view/edit their own profile without admin role
- AC5: API sanitizes all input to prevent injection attacks
- AC6: API implements rate limiting (100 req/min for regular users, 500 for admins)
- AC7: Sensitive fields (passwordHash, mfaSecret) are never returned in API responses
- AC8: All user management operations are logged in audit trail

## FR-USER-BE-020: Caching Strategy
**Requirement Statement**: The system shall implement caching for user data to optimize performance.

**Acceptance Criteria**:
- AC1: User detail queries are cached with 5-minute TTL
- AC2: User list queries are cached with 2-minute TTL
- AC3: Cache keys include tenant identifier and query parameters
- AC4: Cache is invalidated automatically when user is updated
- AC5: User role lookups are cached to optimize authorization checks
- AC6: Cache hit rate is monitored (target 70%+ for read operations)
- AC7: Redis is used for distributed caching across instances
- AC8: Cache failures fall back to database queries gracefully

## FR-USER-BE-021: Performance and Scalability
**Requirement Statement**: The system shall meet performance targets and support scaling to thousands of users per tenant.

**Acceptance Criteria**:
- AC1: User listing API responds within 200ms for 95th percentile
- AC2: User detail API responds within 100ms for 95th percentile
- AC3: User creation API responds within 300ms for 95th percentile
- AC4: User search responds within 300ms for 95th percentile
- AC5: Database queries use appropriate indexes (execution time under 50ms)
- AC6: API supports horizontal scaling with stateless design
- AC7: Database connection pooling is implemented
- AC8: System handles 10,000+ users per tenant without performance degradation
