function CriarCalendario(tipoutilizador) {
    $(document).ready(function () {
        var events = [];
        var selectedEvent = null;
        renderCalendario();
        //pode dar barrada se nao passar o tipo de utilizador...nao me lembro...
        function renderCalendario() {
            events = [];
            $.ajax({
                type: "GET",
                url: "/" + tipoutilizador + "/GetEvents",
                success: function (eventos) {
                    $.each(eventos, function (i, v) {
                        events.push({
                            id : v.id,
                            titulo: v.Titulo,
                            decricao: v.Descricao,
                            inicio: v.Inicio,
                            fim: v.Fim
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
                navLinks: true,

                navLinkDayClick: function (date, jsEvent) {
                    console.log('day', date.format()); // date is a moment
                    console.log('coords', jsEvent.pageX, jsEvent.pageY);
                },
                
                dayClick: function (date, jsEvent, view) {
                    renderCalendario();
                    alert('CLICOU DIA');
                    $('#myModal').modal().hide();
                    $('#MyModalSave').modal();

                },
                eventClick: function (calEvent, jsEvent, view) {
                    renderCalendario();
                    alert('CLICOU DIA');
                    $('#myModal').modal().hide();
                    $('#MyModalSave').modal();

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
        }
    });
}