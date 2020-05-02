using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineChatServer.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public string Address { get; private set; }

        public string RegisterDate { get; set; }

        public string ImagePath { get; set; }

        public string AboutAs { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }

        public ApplicationUser()
        {
            RegisterDate = DateTime.Now.ToString("G");
        }


        public ApplicationUser(string login, string firstName, string lastName, string email, string imagePath) : this()
        {
            SetUserName(login);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetImagePath(imagePath);
        }

        public void SetAboutAs(string info)
        {
            if (string.IsNullOrWhiteSpace(info)) throw new ArgumentException("Inbfo about as is empty", nameof(info));

            AboutAs = info;
        }


        public void SetUserName(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("login is empty", nameof(login));

            UserName = login;
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("firstName is empty", nameof(firstName));

            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("lastName is empty", nameof(lastName));

            LastName = lastName;
        }

        public void SetPhone(string phone) //TODO: валидация на номер
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("phone is empty", nameof(phone));

            PhoneNumber = phone;
        }

        public void SetEmail(string email) //TODO: валидация на почту
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("phone is empty", nameof(email));

            Email = email;
        }

        public void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("address is empty", nameof(address));

            Address = address;
        }

        public void SetImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("ImagePath is empty", nameof(imagePath));

            ImagePath = imagePath;
        }
    }
}