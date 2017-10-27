using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace FitxerHash
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            ConsoleKeyInfo option;
            string fileName;
            string textPla;
            string hash;
            string fileHash;
            string textHash;
            // Menu d'inici
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("************** GENERADOR HASH ********************");
                Console.WriteLine();
                Console.WriteLine("1............. CALCULA HASH FITXER");
                Console.WriteLine("2............. COMPROVA HASH FITXER");
                Console.WriteLine("0............. EXIT");
                Console.WriteLine("**************************************************");
                do
                {
                    // El ReadKey(true) és perquè no s'escrigui per pantalla
                    option = Console.ReadKey(true);
                } while (option.KeyChar < '0' || option.KeyChar > '2');
                Console.Clear();
                // Selecció acció
                switch (option.KeyChar)
                {
                    // Calcular hash d'arxiu i emmagatzemar
                    // Si no es pot llegir el fitxer o si hi ha un error
                    // calculant el hash saltem directament al final del case
                    case '1':
                        Console.Write("Entra el nom del fitxer de text: ");
                        fileName = Console.ReadLine();
                        textPla = LlegirFitxer(fileName);
                        if (string.IsNullOrEmpty(textPla)) break;
                        string path = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;
                        hash = CalculaHash(textPla);
                        if (string.IsNullOrEmpty(hash)) break;
                        Console.Write("Entra nom pel fitxer de Hash (es guardarà a la mateixa ruta que el fitxer de text): ");
                        fileHash = Console.ReadLine();

                        EscriuFitxer(path+fileHash, hash);
                        break;

                    // Comaparar hash arxiu amb l'arxiu guardat   
                    // Si es produeix qualsevol error al llegir els fitxers
                    // o calculant el hash saltem al final del case
                    case '2':
                        Console.Write("Entra el nom del fitxer de text: ");
                        fileName = Console.ReadLine();
                        textPla = LlegirFitxer(fileName);
                        if (string.IsNullOrEmpty(textPla)) break;
                        textPla = LlegirFitxer(fileName);
                        hash = CalculaHash(textPla);
                        if (string.IsNullOrEmpty(hash)) break;
                        Console.Write("Entra el nom del fitxer de hash: ");
                        fileHash = Console.ReadLine();
                        textHash = LlegirFitxer(fileHash);
                        if (string.IsNullOrEmpty(textHash)) break;
                        if (textHash.Equals(hash))
                            Console.WriteLine("Els hashs coincideixen");
                        else
                            Console.WriteLine("Els Hashs no coincideixen");

                        Console.ReadKey(true);
                        break;
                    // Sortim del programa   
                    case '0':
                        exit = true;
                        break;
                }
            }
        }


        /// <summary>
        /// Retorna un string amb el contingut del fitxer
        /// </summary>
        /// <param name="nomFitxer"> és el nom de l'arxiu en format string</param>
        /// <returns>retorna un string amb el contingut del fitxer o null si hi ha error</returns>
        static string LlegirFitxer(string nomFitxer)
        {
            try
            {
                return File.ReadAllText(nomFitxer);
            }
            catch (Exception)
            {
                Console.WriteLine("No es troba el fitxer o no es pot llegir");
                Console.ReadKey(true);
                return null;
            }
        }

        /// <summary>
        /// Escriu com fitxer text l'string que se li passa
        /// </summary>
        /// <param name="nomFitxer"> nom de l'arxiu que volem crear</param>
        /// <param name="textHash"> string que conté el que es vol guardar</param>
        static void EscriuFitxer(string nomFitxer, string textHash)
        {
            try
            {
                File.WriteAllText(nomFitxer, textHash);
            }
            catch (Exception)
            {
                Console.WriteLine("No s'ha pogut escriure el fitxer");
                Console.ReadKey(true);

            }
        }

        /// <summary>
        /// Retorna el hash en format string de l'string paràmetre
        /// </summary>
        /// <param name="textIn"> és el text del qual volem calcular el hash</param>
        /// <returns> retorna un string amb el hash resultat o null si hi ha error</returns>
        static string CalculaHash(string textIn)
        {

            try
            {
                // Convertim l'string a un array de bytes
                byte[] bytesIn = UTF8Encoding.UTF8.GetBytes(textIn);
                // Instanciar classe per fer hash
                SHA512Managed SHA512 = new SHA512Managed();
                // Calcular hash
                byte[] hashResult = SHA512.ComputeHash(bytesIn);

                // Si volem mostrar el hash per pantalla o guardar-lo en un arxiu de text
                // cal convertir-lo a un string

                String textOut = BitConverter.ToString(hashResult, 0);


                // Eliminem la classe instanciada
                SHA512.Dispose();
                return textOut;
            }
            catch (Exception)
            {
                Console.WriteLine("Error calculant el hash");
                Console.ReadKey(true);
                return null;
            }
        }
    }
}
