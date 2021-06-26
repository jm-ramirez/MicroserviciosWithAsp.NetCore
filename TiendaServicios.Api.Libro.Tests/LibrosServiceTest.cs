﻿using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(X => X.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        [Fact]
        public async void GetLibros()
        {
            System.Diagnostics.Debugger.Launch();
            //Que metodo dentro de mi microservice libro se esta encargando
            //de realizar la consulta de libros de la base de datos???

            //1. Emular a la instancia de entity framework core - ContextoLiberia
            //Para emular las aciones y eventos de un objeto en un ambiente de unit test
            //utilizamos objetos de tipo mock

            var mockContexto = CrearContexto();

            //2. Emular al mapping IMapper

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();
            //3. Instanciar a la clase Manejador y pasarle como parámetros los mocks que eh creado.


            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }
    }
}
