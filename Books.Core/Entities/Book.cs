using Books.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Books.Core.Entities
{
    public class Book : EntityObject, IValidatableObject
    {

        public ICollection<BookAuthor> BookAuthors { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Titel muss definiert sein")]
        [MaxLength(200, ErrorMessage = "Titel darf maximal 200 Zeichen lang sein")]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string Publishers { get; set; }

        [IsbnValidation]
        [Required, MaxLength(13)]
        public string Isbn { get; set; }

        public Book()
        {
            BookAuthors = new List<BookAuthor>();
        }

        /// <summary>
        /// Eine gültige ISBN-Nummer besteht aus den Ziffern 0, ... , 9,
        /// 'x' oder 'X' (nur an der letzten Stelle)
        /// Die Gesamtlänge der ISBN beträgt 10 Zeichen.
        /// Für die Ermittlung der Prüfsumme werden die Ziffern 
        /// von rechts nach links mit 1 - 10 multipliziert und die 
        /// Produkte aufsummiert. Ist das rechte Zeichen ein x oder X
        /// wird als Zahlenwert 10 verwendet.
        /// Die Prüfsumme muss modulo 11 0 ergeben.
        /// </summary>
        /// <returns>Prüfergebnis</returns>
        public static bool CheckIsbn(string isbn)
        {
            bool isValid = false;
            if (isbn == null)
            {
                return false;
                throw new ArgumentNullException(nameof(Isbn));
            }

            if (isbn.Length != 10)
            {
                return false;
                throw new Exception("Isbn has not 10 chars!");
            }

            int sum = 0;
            for (int i = isbn.Length - 1; i >= 0; i--)
            {

                if (isbn.ToLower()[i].Equals('x'))
                {
                    sum += 10 * (i + 1);
                }
                else
                {
                    int digit = Convert.ToInt32(isbn[i]) - '0';
                    sum += digit * (i + 1);
                }
            }

            if (sum % 11 == 0)
            {
                isValid = true;
            }
            return isValid;
        }


        public override string ToString()
        {
            return $"{Title} {Isbn} mit {BookAuthors.Count()} Autoren";
        }

        /// <inheritdoc />
        /// <summary>
        /// Jedes Buch muss zumindest einen Autor haben.
        /// Weiters darf ein Autor einem Buch nicht mehrfach zugewiesen
        /// werden.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidationResult result = new IsbnValidation().GetValidationResult(Isbn, new ValidationContext(Isbn));

            if (result != null && result != ValidationResult.Success)
            {
                yield return new ValidationResult(result.ErrorMessage, new string[] { nameof(Isbn) });
            }

            if (BookAuthors.Count == 0)
            {
                yield return new ValidationResult("Book must have at least one author", new string[] { nameof(BookAuthors) });
            }

            var duplicates = BookAuthors.GroupBy(_ => _.Author)
                                       .Where(_ => _.Count() > 1)
                                       .Select(_ => _.Key);
            foreach (var duplicate in duplicates)
            {
                yield return new ValidationResult($"{duplicate.Name} are twice authors of the book", new string[] { nameof(BookAuthors) });
            }
        }
    }
}

