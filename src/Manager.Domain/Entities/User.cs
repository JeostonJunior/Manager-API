using Manager.Core.Exceptions;
using Manager.Domain.Validators;

namespace Manager.Domain.Entities
{
    public class User : Base
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        //EF
        protected User() { }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            _errors = new List<string>();
            Validate();
        }

        public override bool Validate()
        {
            var validationUser = new UserValidator();
            var validation = validationUser.Validate(this);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    _errors.Add(error.ErrorMessage);
                }

                throw new DomainExceptions("Some contents are invalid, please correct them", _errors);
            }

            return true;
        }
    }
}
