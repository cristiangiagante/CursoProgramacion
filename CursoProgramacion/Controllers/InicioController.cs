using CursoDeProgramacion.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CursoDeProgramacion.Controllers
{
    public class InicioController : Controller
    {
        public string DatabaseName { get; set; } = "bd.json";
        public static string DbPath { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            if(string.IsNullOrEmpty(DbPath))
            {
                DbPath = HttpContext.Server.MapPath($"~/jsonDb/{DatabaseName}");
                if (!System.IO.File.Exists(DbPath))
                {
                    ClasesDiarias EmptyDB = new ClasesDiarias();
                    EmptyDB.ListaClases = new List<ClaseDiaria>();
                    SaveIntoJsonDB(EmptyDB);
                }
            }

            var model = ObtenerJsonDB();
            return View(model.ListaClases);
        }

        [HttpGet]
        public ActionResult Edit(int id, string passPhrase)
        {
            if (passPhrase == "unaclave")
            {
                var model = ObtenerJsonDB();
                return View("EditClase", model.ListaClases[id]);
            }
            return View();
            
        }

        [HttpPost]
        public ActionResult Edit(ClaseDiaria clase)
        {
            var model = ObtenerJsonDB();
            model.ListaClases[clase.Id] = clase;
            SaveIntoJsonDB(model);
            return View(clase);
        }

        public ActionResult Create(string passPhrase)
        {
            if (passPhrase == "unaclave")
            {
                return View("CrearClase");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClaseDiaria clase)
        {

            if (ModelState.IsValid)
            {
                AgregarIntoJsonDB(clase);
            }
            return RedirectToAction("Index");
        }

        private void SaveIntoJsonDB(ClasesDiarias list)
        {
            System.IO.File.Delete(DbPath);
            var jsonDBObject = Json(list);
            string result = new JavaScriptSerializer().Serialize(jsonDBObject.Data);
            StreamWriter bd = System.IO.File.CreateText(DbPath);
            bd.WriteLine(result);
            bd.Close();
        }

        private void AgregarIntoJsonDB(ClaseDiaria clase)
        {
            var jsonDbStream = System.IO.File.OpenText(DbPath);
            var jsonDbRaw = jsonDbStream.ReadToEnd();
            jsonDbStream.Close();
            var jsonDb = new JavaScriptSerializer().Deserialize<ClasesDiarias>(jsonDbRaw);
            var cantidad = jsonDb.ListaClases.Count;
            clase.Id += cantidad;
            jsonDb.ListaClases.Add(clase);
            SaveIntoJsonDB(jsonDb);
        }

        private ClasesDiarias ObtenerJsonDB()
        {
            var jsonDbStream = System.IO.File.OpenText(DbPath);
            var jsonDbRaw = jsonDbStream.ReadToEnd();
            jsonDbStream.Close();
            var jsonDb = new JavaScriptSerializer().Deserialize<ClasesDiarias>(jsonDbRaw);
            return jsonDb;
        }
    }
}