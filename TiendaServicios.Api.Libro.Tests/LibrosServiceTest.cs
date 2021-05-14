using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        [Fact]
        public void GetLibros()
        {
            //Que metodo dentro de mi microservice libro se esta encargando
            //de realizar la consulta de libros de la base de datos???

            //1. Emular a la instancia de entity framework core - ContextoLiberia
            //Para emular las aciones y eventos de un objeto en un ambiente de unit test
            //utilizamos objetos de tipo mock

            var mockContexto = new Mock<ContextoLibreria>();

            //2. Emular al mapping IMapper

            var mockMapper = new Mock<IMapper>();

            //3. Instanciar a la clase Manejador y pasarle como parámetros los mocks que eh creado.


            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mockMapper.Object);
        }
    }
}
