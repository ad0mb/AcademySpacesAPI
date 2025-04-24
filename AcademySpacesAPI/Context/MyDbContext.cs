using System;
using System.Collections.Generic;
using AcademySpacesAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AcademySpacesAPI.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<FacultyRole> FacultyRoles { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=mysql6008.site4now.net;database=db_ab66bb_staging;uid=ab66bb_staging;pwd=PO6!^DDF67y", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("classrooms");

            entity.HasIndex(e => e.ClassroomTeacherId, "classrooms_faculty_faculty_id_fk");

            entity.HasIndex(e => e.SchoolId, "classrooms_schools_school_id_fk");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
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
            entity.Property(e => e.Create).HasColumnName("create");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.Delete).HasColumnName("delete");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(150)
                .HasColumnName("permission_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Update).HasColumnName("update");

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

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("students");

            entity.HasIndex(e => e.ClassId, "students_classrooms_class_id_fk");

            entity.HasIndex(e => e.ParentId, "students_parents_parent_id_fk");

            entity.HasIndex(e => e.IdentityId, "students_pk_2").IsUnique();

            entity.HasIndex(e => e.RoleId, "students_roles_role_id_fk");

            entity.HasIndex(e => e.SchoolId, "students_schools_school_id_fk");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
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
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("students_classrooms_class_id_fk");

            entity.HasOne(d => d.Parent).WithMany(p => p.Students)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("students_parents_parent_id_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.Students)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_roles_role_id_fk");

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_schools_school_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
