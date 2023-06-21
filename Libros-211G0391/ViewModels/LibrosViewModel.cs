using GalaSoft.MvvmLight.Command;
using Libros_211G0391.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Libros_211G0391.ViewModels
{
    public class LibrosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        int indice;

        public string Error { get; set; } = "";
        public ObservableCollection<Libro> Libros { get; set; } = new ObservableCollection<Libro>();

        public Libro libro { get; set; }

        public Libro Libro
        {
            get { return libro; }
            set
            {
                libro = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }


        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Libro.Titulo))
            {
                Error += "Escriba el nombre del contacto\n";
            }
            if (string.IsNullOrEmpty(Libro.TituloOriginal))
            {
                Error += "Escriba la dirección del contacto\n";
            }
            if (string.IsNullOrEmpty(Libro.Autor))
            {
                Error += "Escriba el correo electrónico del contacto\n";
            }
            if (Libro.FehcaDePublicacion.Date > DateTime.Now.Date)
            {
                Error += "La fecha es errónea\n";
            }


            if (Error == "" && Libros != null)
            {
                Libros.Add(Libro);
                Guardar();
                Vista = "ver";
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;

            if (vistaACambiar == "agregar")
            {
                Libro = new Libro();
            }
            if (vistaACambiar == "editar")
            {
                if (Libros != null)
                {
                    indice = Libros.IndexOf(Libro);
                }
                Libro copiaLibro = new Libro()
                {
                    Autor = Libro.Autor,
                    FehcaDePublicacion = Libro.FehcaDePublicacion,
                    Reseña = Libro.Reseña,
                    Titulo = Libro.Titulo,
                    TituloOriginal = Libro.TituloOriginal,
                    URLPortada = Libro.URLPortada
                };
                Libro = copiaLibro;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));

        }

        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }


        public void Editar()
        {
            if (Libros != null)
            {
                Libros[indice] = Libro;
                Guardar();
                CambiarVista("ver");

            }

        }

        public void Eliminar()
        {
            if (Libro != null && Libros != null)
            {
                Libros.Remove(Libro);
                CambiarVista("ver");
                Guardar();
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Libros);
            File.WriteAllText("Libros.json", json);

        }

        public void Cargar()
        {
            if (File.Exists("Libros.json"))
            {
                var json = File.ReadAllText("Libros.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(json);
                if (datos != null)
                {
                    Libros = (ObservableCollection<Libro>)datos;
                }
                else
                {
                    Libros = new ObservableCollection<Libro>();

                }
            }
        }

        public LibrosViewModel()
        {

            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }

    }
}
