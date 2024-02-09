using SimpleRecords.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleRecords.Common.Models
{
    public class PersonDetails
    {
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Favorite color is required")]
        public string FavoriteColor { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [ValidAge]
        public DateTime BirthDate { get; set; }

        public static PersonDetails FromDelimitedString(string text, string delimiter = ", ")
        {
            string[] separators = { delimiter };
            string[] parts = text.Split( separators, StringSplitOptions.None );

            if (parts.Length != 5)
                throw new Exception("Delimited text does not contain correct number of fields.");

            char[] trimCharacters = { '"', '\'' };

            return new PersonDetails()
            {
                FirstName = parts[1].Trim(trimCharacters),
                LastName = parts[0].Trim(trimCharacters),
                Email = parts[2].Trim(trimCharacters),
                FavoriteColor = parts[3].Trim(trimCharacters),
                BirthDate = DateTime.Parse(parts[4].Trim(trimCharacters))
            };
        }
    }
}
