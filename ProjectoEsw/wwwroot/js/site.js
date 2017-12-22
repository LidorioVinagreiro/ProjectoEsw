function CriarCalendario(tipoutilizador,perfil) {
    $(document).ready(function () {
        var events = [];
        var selectedEvent = null;
        var _perfil = perfil;
        renderCalendario();
        //pode dar barrada se nao passar o tipo de utilizador...nao me lembro...
        function renderCalendario() {
            events = [];

            $.ajax({
                type: "GET",
                url: "/" + tipoutilizador + "/GetEvents",
                contentType: 'application/json',
                data: { idPerfil: perfil },
                dataType: 'json',
                success: function (data) {
                    $.each(data, function (i, v) {
                        events.push({
                            eventID: v.id,
                            Title : v.Titulo,
                            Description : v.Descricao,
                            Start : v.Inicio,
                            End: v.Fim,
                            PerfilFk: v.PerfilFK
                        });
                    })
                    GerarCalendario(events);
                },
                error: function (error) {
                    alert("FALHOU ALGO");
                }
            });
        };

        function GerarCalendario(eventos) {
            //os eventos do fullcalender ja tem propriedades
            $('#calendar').fullCalendar('destroy');
            $('#calendar').fullCalendar({
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek'
                },
                themeSystem: 'bootstrap3',
                bootstrapGlyphicons: {
                    close: 'glyphicon-remove',
                    prev: 'glyphicon-chevron-left',
                    next: 'glyphicon-chevron-right',
                    prevYear: 'glyphicon-backward',
                    nextYear: 'glyphicon-forward'
                },
                editable: true,

                buttonText: {
                    today: 'Hoje',
                    month: 'Mês',
                    week: 'Semana',
                    day: 'Dia'
                },

                // quando se passa o rato por cima de um evento
                eventMouseover: function (event, jsEvent, view) {
                    if (view.name !== 'agendaDay') {
                        $(jsEvent.target).attr('title', event.titulo);
                    }
                },

                events: eventos,
                
                dayClick: function (date, jsEvent, view) {                   
                    var dataSelecionada = new Date(date.format('MM/DD/YYYY')); 
                    $('#InicioInput').datetimepicker({
                        format: 'DD/MM/YYYY HH:mm',
                        inline: true,
                        sideBySide:true
                    });
                    $('#InicioInput').data("DateTimePicker").date(dataSelecionada);

                    $('#FimInput').datetimepicker({
                        format: 'DD/MM/YYYY HH:mm',
                        inline: true,
                        sideBySide: true
                    });
                    $('#FimInput').data("DateTimePicker").date(dataSelecionada);                    
                    $("#myModalSave").modal();

                },
                eventClick: function (calEvent, jsEvent, view) {
                    $("#myModalSave").modal();

                }

                //quando se clica num evento
                //eventClick: function (calEvent, jsEvent, view) {
                //    selectedEvent = calEvent;
                //    $('#myModal #eventoTitulo').text(calEvent.Titulo);
                //    var $description = $('<div />');
                //    $description.append($('<p />').html('<b>Inicio:</b>' + calEvent.start.format("DD-MMM-YYYY HH:mm a")));
                //    if (calEvent.end != null) {
                //        $description.append($('<p/>').html('<b>Fim:</b>' + calEvent.end.format("DD-MMM-YYYY HH:mm a")));
                //    }
                //    $description.append($('<p />').html('<b>Description:</b>' + calEvent.description));
                //    $('#myModal #pDetails').empty().html($description);

                //    $('#myModal').modal();
                //}
                //quando se clica num dia
                
            });

            $('#btnDelete').click(function () {
                if (selectedEvent != null && confirm("apagar evento selecionado?")) {
                    $.ajax({
                        type: 'POST',
                        url: '/' + tipoUtilizador + '/DeleteEvent',
                        data: { 'id': selectedEvent.eventID },
                        success: function (data) {
                            if (data.status) {
                                //refazer calendario
                                renderCalendario();
                                $('#myModal').modal().hide();
                            } 
                        },
                        error: function () {
                            alert("OCORREU UM ERRO AO APAGAR EVENTO");
                        }
                    });
                } else {
                    alert('nao ha evento selecionado');
                }
            });
            

            $('#btnEditar').click(function () {

            });

            $('#btnSave').click(function () {
                var data1 = $('#InicioInput').val();
                var data2 = $('#FimInput').val();
                var descricao = $('#DescricaoInput').val();
                var titulo = $('#Titulo').val();
                var tipo = $('#Tipo').find(":selected").text();
                var data2GreaterData1 = moment(data2).diff(moment(data1)) > 0;
                if (!data2GreaterData1 || !titulo || !descricao) {
                    alert("Falta inserir Titulo ou Descricao ou a Data final nao é superior à inicial");                    

                } else {
                    var evento = {
                        Descricao : descricao,
                        Inicio : moment(data1),
                        Fim : moment(data2),
                        Titulo : titulo,
                        PerfilFK : _perfil
                    };
                    SaveEvents(evento);
                    $('#myModalSave').modal().hide();    
                }    

            });

            function SaveEvents(evento) {
                $.ajax({
                    type: "POST",
                    url: "/" + tipoutilizador + "/SaveEvents",
                    contentType: 'application/json',
                    data: JSON.stringify(evento),
                    success: function (status) {
                        if (!status) {
                            alert("Algo correu mal");
                        } else {
                            alert("Evento guardado");
                        }
                    },error: function (error) {
                        alert("Erro a gravar evento");
                    }

                })
            };

        }
    });
}