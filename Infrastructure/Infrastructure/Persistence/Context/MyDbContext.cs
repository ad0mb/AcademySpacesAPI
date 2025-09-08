using System;
using System.Collections.Generic;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Infrastructure.Infrastructure.Persistence.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<ClassroomStudent> ClassroomStudents { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<FacultyRole> FacultyRoles { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<SchoolConfiguration> SchoolConfigurations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentParent> StudentParents { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UserAppPeference> UserAppPeferences { get; set; }

    public virtual DbSet<YearLevel> YearLevels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.12.113;database=ABDI;uid=root;pwd=Abdirauuf2004!;allowloadlocalinfile=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("announcements");

            entity.HasIndex(e => e.SenderId, "Announcments_faculty_faculty_id_fk");

            entity.HasIndex(e => e.SchoolId, "Announcments_schools_school_id_fk");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnnouncmentId)
                .HasColumnType("text")
                .HasColumnName("announcment_ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.Priority)
                .HasMaxLength(255)
                .HasColumnName("priority");
            entity.Property(e => e.SchoolId).HasColumnName("School_ID");
            entity.Property(e => e.SenderId).HasColumnName("sender_ID");
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.School).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Announcments_schools_school_id_fk");

            entity.HasOne(d => d.Sender).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Announcments_faculty_faculty_id_fk");

            entity.HasMany(d => d.Tags).WithMany(p => p.Announcements)
                .UsingEntity<Dictionary<string, object>>(
                    "AnnouncementTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_at_tag"),
                    l => l.HasOne<Announcement>().WithMany()
                        .HasForeignKey("AnnouncementId")
                        .HasConstraintName("fk_at_announcement"),
                    j =>
                    {
                        j.HasKey("AnnouncementId", "TagId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("announcement_tags");
                        j.HasIndex(new[] { "TagId", "AnnouncementId" }, "ix_at_tag_id_announcement_id");
                        j.IndexerProperty<int>("AnnouncementId").HasColumnName("announcement_id");
                        j.IndexerProperty<ulong>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PRIMARY");

            entity.ToTable("classrooms");

            entity.HasIndex(e => e.ClassroomTeacherId, "classrooms_faculty_faculty_id_fk");

            entity.HasIndex(e => e.SchoolId, "classrooms_schools_school_id_fk");

            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.ClassroomName)
                .HasMaxLength(255)
                .HasColumnName("classroom_name");
            entity.Property(e => e.ClassroomTeacherId).HasColumnName("classroom_teacher_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.ClassroomTeacher).WithMany(p => p.Classrooms)
                .HasForeignKey(d => d.ClassroomTeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("classrooms_faculty_faculty_id_fk");

            entity.HasOne(d => d.School).WithMany(p => p.Classrooms)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("classrooms_schools_school_id_fk");
        });

        modelBuilder.Entity<ClassroomStudent>(entity =>
        {
            entity.HasKey(e => new { e.ClassroomId, e.StudentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("classroom_students");

            entity.HasIndex(e => e.StudentId, "classroom_students_students_student_id_fk");

            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_updated");

            entity.HasOne(d => d.Classroom).WithMany(p => p.ClassroomStudents)
                .HasForeignKey(d => d.ClassroomId)
                .HasConstraintName("classroom_students_classrooms_classroom_id_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.ClassroomStudents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("classroom_students_students_student_id_fk");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.HasIndex(e => e.SchoolId, "courses_schools_school_id_fk");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(10)
                .HasColumnName("course_code");
            entity.Property(e => e.CourseDescription)
                .HasMaxLength(255)
                .HasColumnName("course_description");
            entity.Property(e => e.CourseName)
                .HasMaxLength(150)
                .HasColumnName("course_name");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.School).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("courses_schools_school_id_fk");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PRIMARY");

            entity.ToTable("faculty");

            entity.HasIndex(e => e.IdentityId, "faculty_pk").IsUnique();

            entity.HasIndex(e => e.SchoolId, "faculty_schools_school_id_fk");

            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IdentityId).HasColumnName("identity_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.School).WithMany(p => p.Faculties)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("faculty_schools_school_id_fk");
        });

        modelBuilder.Entity<FacultyRole>(entity =>
        {
            entity.HasKey(e => new { e.FacultyId, e.RoleId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("faculty_roles");

            entity.HasIndex(e => e.RoleId, "faculty_roles_roles_role_id_fk");

            entity.Property(e => e.FacultyId).HasColumnName("faculty_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");

            entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyRoles)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("faculty_roles_faculty_faculty_id_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.FacultyRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("faculty_roles_roles_role_id_fk");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PRIMARY");

            entity.ToTable("organizations");

            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PRIMARY");

            entity.ToTable("parents");

            entity.HasIndex(e => e.IdentityId, "parents_pk").IsUnique();

            entity.HasIndex(e => e.RoleId, "parents_roles_role_id_fk");

            entity.HasIndex(e => e.SchoolId, "parents_schools_school_id_fk");

            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IdentityId).HasColumnName("identity_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Parents)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("parents_roles_role_id_fk");

            entity.HasOne(d => d.School).WithMany(p => p.Parents)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("parents_schools_school_id_fk");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.SchoolId, "Payments_schools_school_id_fk");

            entity.HasIndex(e => e.StudentId, "Payments_students_student_id_fk");

            entity.HasIndex(e => e.ReceiptNumber, "ReceiptNumber").IsUnique();

            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Feetype).HasMaxLength(50);
            entity.Property(e => e.Paymentmethod).HasMaxLength(50);
            entity.Property(e => e.ReceiptNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentFirstName).HasMaxLength(50);
            entity.Property(e => e.StudentLastName).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Payments_schools_school_id_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("Payments_students_student_id_fk");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PRIMARY");

            entity.ToTable("periods");

            entity.HasIndex(e => e.CourseId, "periods_courses_course_id_fk");

            entity.HasIndex(e => e.TeacherId, "periods_faculty_faculty_id_fk");

            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .HasColumnName("location");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Periods)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("periods_courses_course_id_fk");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Periods)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("periods_faculty_faculty_id_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.SchoolId, "roles_schools_school_id_fk");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(255)
                .HasColumnName("role_description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.School).WithMany(p => p.Roles)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("roles_schools_school_id_fk");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role_permissions");

            entity.HasIndex(e => e.RoleId, "role_permissions_roles_role_id_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CanCreate).HasColumnName("can_create");
            entity.Property(e => e.CanDelete).HasColumnName("can_delete");
            entity.Property(e => e.CanUpdate).HasColumnName("can_update");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(150)
                .HasColumnName("permission_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role_permissions_roles_role_id_fk");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PRIMARY");

            entity.ToTable("schools");

            entity.HasIndex(e => e.OrganizationId, "schools_organizations_organization_id_fk");

            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            entity.Property(e => e.CountryOfOrigin)
                .HasMaxLength(255)
                .HasColumnName("country_of_origin");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

            entity.HasOne(d => d.Organization).WithMany(p => p.Schools)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("schools_organizations_organization_id_fk");
        });

        modelBuilder.Entity<SchoolConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("school_configuration");

            entity.HasIndex(e => e.SchoolId, "school_configuration_schools_school_id_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            entity.Property(e => e.StaticClassroom).HasColumnName("static_classroom");

            entity.HasOne(d => d.School).WithMany(p => p.SchoolConfigurations)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("school_configuration_schools_school_id_fk");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.IdentityId, "students_pk_2").IsUnique();

            entity.HasIndex(e => e.RoleId, "students_roles_role_id_fk");

            entity.HasIndex(e => e.SchoolId, "students_schools_school_id_fk");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IdentityId).HasColumnName("identity_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Students)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_roles_role_id_fk");

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_schools_school_id_fk");
        });

        modelBuilder.Entity<StudentParent>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.ParentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("student_parents");

            entity.HasIndex(e => e.ParentId, "student_parents_parents_parent_id_fk");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");

            entity.HasOne(d => d.Parent).WithMany(p => p.StudentParents)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("student_parents_parents_parent_id_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentParents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("student_parents_students_student_id_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Name, "uq_tags_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserAppPeference>(entity =>
        {
            entity.HasKey(e => e.IdentityId).HasName("PRIMARY");

            entity.ToTable("user_app_peferences");

            entity.Property(e => e.IdentityId).HasColumnName("identity_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Locale)
                .HasMaxLength(50)
                .HasDefaultValueSql("'en'")
                .HasColumnName("locale");
            entity.Property(e => e.PageBrightness)
                .HasMaxLength(50)
                .HasDefaultValueSql("'system'")
                .HasColumnName("page_brightness");
        });

        modelBuilder.Entity<YearLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("year_levels");

            entity.HasIndex(e => e.SchoolId, "year_levels_schools_school_id_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.HierarchalLevel).HasColumnName("hierarchal_level");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            entity.Property(e => e.YearLevelCode)
                .HasMaxLength(50)
                .HasColumnName("year_level_code");
            entity.Property(e => e.YearLevelName)
                .HasMaxLength(100)
                .HasColumnName("year_level_name");

            entity.HasOne(d => d.School).WithMany(p => p.YearLevels)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("year_levels_schools_school_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
