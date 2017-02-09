namespace Blog.Business.Entity
{
    using System;

    public class UserView
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual bool Disabled { get; set; }

        public virtual string RoleName { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual DateTime LastActivityDate { get; set; }
    }
}