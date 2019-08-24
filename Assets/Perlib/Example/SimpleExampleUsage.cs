using UnityEngine;
using SardonicMe.Perlib;
using System.IO;
using System;

#pragma warning disable 219, 649

namespace PerlibExamples
{

    // This will show you basic usage of a Perlib.
    // Please follow the comments for explanations.

    // Note that if you are building for Web platforms,
    // Perlib will fallback to using PlayerPrefs instead of files
    // due to Unity's file system restrictions on these platforms.
    // No features are affected by this and you can use Perlib the same way. Everything will work.

    public class SimpleExampleUsage : MonoBehaviour
    {

        void Awake()
        {

            // Create our Perlib.
            // We pass a filename, and an optional password used for encryption.
            // By default, Perlib will use Application.persistentDataPath for its path.

            Perlib savefile = new Perlib("MyAwesomeGame.sav", "csDKndc30ns");


            // You can also use the overloaded constructor to specify a FileInfo.
            // This will instruct Perlib to use a location you chose:

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Perlib Example\MyAwesomeGame.sav";
            savefile = new Perlib(new FileInfo(savePath), "csDKndc30ns");


            // Now we can open the Perlib. If the Perlib can't find a file at the path specified,
            // it will attempt to create a new one.

            savefile.Open();


            // That's it. Now you can begin saving any type that can be serialized
            savefile.SetValue("Highscore", 5318008);
            savefile.SetValue("Dexterity", 8.7f);
            savefile.SetValue("Fullscreen", true);
            savefile.SetValue("Name", "Moogledoop the Merciless");


            // Including custom classes
            MyClass myObject = new MyClass();
            myObject.Point = new Vector3(34f, 12f, 7f);

            savefile.SetValue("My Object", myObject);


            // And get them back
            int Highscore       = savefile.GetValue<int>("Highscore");
            float Dexterity     = savefile.GetValue<float>("Dexterity");
            bool Fullscreen     = savefile.GetValue<bool>("Fullscreen");
            string Name         = savefile.GetValue<string>("Name");
            MyClass LoadedObj   = savefile.GetValue<MyClass>("My Object");


            // If a key is not found, a default value can instead be returned

            // This will return 0, if not found
            int x = savefile.GetValue<int>("Population");

            // This will return 55378008, if not found
            int y = savefile.GetValue<int>("Population", 55378008); 


            // You can also optionally encrypt individual values.
            savefile.SetValue("Encrypted Highscore", 5318008, "x2eEEE3c");


            // The same password will be required next time you try to get the value.
            int decryptedHighscore = savefile.GetValue<int>("Encrypted Highscore", 0, "x2eEEE3c");


            // Most of Perlib was designed to mimic the PlayerPrefs interface
            if (!savefile.HasKey("Highscore"))
                savefile.SetValue("Highscore", 0);

            // Make sure to Save the Perlib to persist changes
            savefile.Save();
        }
    }

    public class MyClass
    {
        public int X, Y, Z;
        public Vector3 Point;
    }

}

#pragma warning restore 219, 649