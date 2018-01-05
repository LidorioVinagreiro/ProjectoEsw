function CriarCalendario(tipoutilizador,perfil) {
    $(document).ready(function () {
        var eventsAux = [];
        var selectedEvent = null;
        var _perfil = perfil;
        renderCalendario();
        //pode dar barrada se nao passar o tipo de utilizador...nao me lembro...
        function renderCalendario() {
            $.ajax({
                type: "GET",
                url: "/" + tipoutilizador + "/GetEvents",
                contentType: 'application/json',
                data: { idPerfil: perfil },
                dataType: 'json',
                success: function (data) {
                    eventsAux = [];
                    $.each(data, function (i, v) {
                        eventsAux.push({
                            id: v.id,
                            title: v.titulo,
                            description: v.descricao,
                            start: v.inicio,
                            end: v.fim,
                            PerfilFk: v.perfilfK
                        });
                    });
                    $('#btnSave').unbind();
                    $('#btnDelete').unbind();
                    $('#btnEditar').unbind();
                    GerarCalendario(eventsAux);
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

                events: eventsAux,
                
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
                    $('#btnDelete').unbind();
                    $('#btnEditar').unbind();
                    selectedEvent = calEvent;
                    $('#inicioEvento').text(calEvent.start);
                    $('#fimEvento').text(calEvent.end);
                    $('#descricaoEvento').text(calEvent.description);

                    $('#btnDelete').click(function () {
                        if (selectedEvent != null && confirm("apagar evento selecionado?")) {
                            $.ajax({
                                type: 'POST',
                                url: '/' + "Candidato" + '/DeleteEvent',
                                data: { 'id': selectedEvent.id },
                                success: function (data) {
                                    if (data.status) {
                                        $('#myModal').modal('hide');
                                        selectedEvent = null;
                                        renderCalendario();
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
                        $('#myModal').modal('hide');
                        var data1 = moment(selectedEvent.start);
                        var data2 = moment(selectedEvent.end);
                        var dataSelecionada1 = new Date(data1);
                        $('#InicioInput').datetimepicker({
                            format: 'DD/MM/YYYY HH:mm',
                            inline: true,
                            sideBySide: true,
                            defaultDate: moment(selectedEvent.start)
                        });
                        //$('#InicioInput').data("DateTimePicker").date(data1);

                        var dataSelecionada2 = new Date(data2);
                        $('#FimInput').datetimepicker({
                            format: 'DD/MM/YYYY HH:mm',
                            inline: true,
                            sideBySide: true,
                            defaultDate: moment(selectedEvent.end)
                        });
                       //$('#FimInput').data("DateTimePicker").date(data2);

                        $('#Descricao').text(selectedEvent.description);
                        $('#Titulo').val(selectedEvent.title);
                        $('#myModalSave').modal();
                    });

                    $("#myModal").modal();
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

            //$('#btnDelete').click(function () {
            //    if (selectedEvent != null && confirm("apagar evento selecionado?")) {
            //        $.ajax({
            //            type: 'POST',
            //            url: '/' + tipoUtilizador + '/DeleteEvent',
            //            data: { 'id': selectedEvent.eventID },
            //            success: function (data) {
            //                if (data.status) {
            //                    renderCalendario();
            //                    $('#myModal').modal().hide();
            //                } 
            //            },
            //            error: function () {
            //                alert("OCORREU UM ERRO AO APAGAR EVENTO");
            //            }
            //        });
            //    } else {
            //        alert('nao ha evento selecionado');
            //    }
            //});
            
            
            $('#btnSave').click(function () {
                var data1 = moment($('#InicioInput').val(), "MM/DD/YYYY HH:mm");
                var data2 = moment($('#FimInput').val(), "MM/DD/YYYY HH:mm");
                var data1String = $('#InicioInput').val();
                var data2String = $('#FimInput').val();
                var descricao = $('#DescricaoInput').val();
                var titulo = $('#Titulo').val();
                var tipo = $('#Tipo').find(":selected").text();
                var diff = data2.diff(data1);
                if (!diff || !titulo || !descricao) {
                    alert("Falta inserir Titulo ou Descricao ou a Data final nao é superior à inicial");
                    alert("data1: " + data1 + " data2: " + data2 + " datadiff: " + (moment(data2).diff(moment(data1))).toString() +""+ titulo + " " + descricao);
                } else {
                    var evento = {
                        descricao: descricao,
                        inicio: data1String,
                        fim: data2String,
                        titulo: titulo,
                        perfilfK: _perfil
                    };
                    SaveEvents(evento);
                    $('#InicioInput').data("DateTimePicker").date();
                    $('#FimInput').data("DateTimePicker").date();
                    $('#DescricaoInput').val = "";
                    $('#Titulo').val = "";
                    $('#myModalSave').modal('hide');    
                }    
            });


            function SaveEvents(evento) {
                $.ajax({
                    type: "POST",
                    url: "/" + tipoutilizador + "/SaveEvents",
                    //dataType: 'json',
                    //contentType: dataType,
                    data: evento,
                    success: function (status) {
                        alert("Evento guardado");
                        renderCalendario();
                    },error: function (error) {
                        alert("Erro a gravar evento");
                    }

                })
            };


        }
    });
}