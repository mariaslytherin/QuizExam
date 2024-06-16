// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.delete-button').click(function () {
        var url = $(this).data('url');
        $('#confirmDelete').attr('href', url);
        $('#confirmDeleteModal').modal('show');

        $('#confirmDelete').off('click').click(function (event) {
            event.preventDefault();  // Prevent the default link click behavior

            $.ajax({
                url: url,
                type: 'POST',
                success: function (result) {
                    $('#confirmDeleteModal').modal('hide');
                    location.reload();
                }
            });
        });
    });

    //Start the Exam
    $('.start-button').click(function () {
        var mode = $(this).data('mode');
        var text = $(this).data('text');
        $('#hiddenMode').val(mode);
        $('#takeExamModal .modal-body p').text(text);
        $('#takeExamModal').modal('show');
    });

    // Filter questions in view exam
    $('input[type=radio][name=filter]').change(function () {
        var selectedValue = this.value;
        var params = new URLSearchParams(window.location.search);
        var id = params.get('takeId');
        
        $.ajax({
            url: '/TakeExam/GetTakeResult',
            type: 'GET',
            data: { filter: selectedValue, takeId: id },
            success: function(response) {
                $('body').html(response);
            }
        });
        
    });
});

var isFinishRequested = false;
$(document).on('click', '#nextButton', function () {
    var timePassed = document.getElementById("timer").innerHTML;
    var model = {
        QuestionId: $("#questionId").val(),
        TakeExamId: $("#takeExamId").val(),
        Order: $("#order").val(),
        IsLast: $("#isLast").val() === 'True',
        CheckedOptionId: $("input[name='CheckedOptionId']:checked").val(),
        ExamId: $("#examId").val(),
        TimePassed: timePassed,
    };

    $.ajax({
        url: "/TakeAnswer/Add",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(model),
        success: function (response) {
            if (response.errorMessage) {
                toastr.error(response.errorMessage);
            } else if (response.isFinished) {
                sessionStorage.removeItem('startTime');
                isFinishRequested = true;
                window.location.href = '/TakeExam/Finish?takeId=' + response.takeExamId + '&timePassed=' + timePassed;
            } else {
                $("#questionForm").html(response);
                $(function () {
                    toastr.success('Успешен запис!');
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            window.location.href = '/';
        }
    });
});

$(document).on('click', '#previousButton', function () {
    var examId = $("#examId").val();
    var takeId = $("#takeExamId").val();
    var order = $("#order").val() - 1;

    $.ajax({
        url: "/Question/GetPreviousQuestion",
        type: "GET",
        data: { 'examId': examId, 'takeId': takeId, 'order': order },
        success: function (response) {
            $("#questionForm").html(response);
        }
    });
});

function StartCountDownTimer(checkTime, timeRemaining) {
    var endTime = Number(sessionStorage.getItem('endTime'));
    if (!endTime) {
        if (timeRemaining) {
            // Convert timeRemaining from HH:MM:SS format to milliseconds
            var timeParts = timeRemaining.split(':');
            var timeInMilliseconds = (+timeParts[0]) * 60 * 60 * 1000 + (+timeParts[1]) * 60 * 1000 + (+timeParts[2]) * 1000;
            endTime = new Date().getTime() + timeInMilliseconds;
        } else {
            endTime = new Date().getTime();
        }
        sessionStorage.setItem('endTime', endTime);
    }
    if (checkTime.length == 0) {
        var x = setInterval(function () {
            var now = new Date().getTime();
            var t = endTime - now;
            var hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((t % (1000 * 60)) / 1000);
            // Add a '0' in front of minutes and seconds if they are less than 10
            hours = hours < 10 ? '0' + hours : hours;
            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;

            if (hours != '00' || minutes != '00' || seconds != '00') {
                document.getElementById("timer").innerHTML = hours + ":" + minutes + ":" + seconds;
            } else {
                clearInterval(checkTime[0]);
                checkTime = [];
                sessionStorage.removeItem('endTime');
                document.getElementById("timer").innerHTML = "Времето изтече!";
                isFinishRequested = true;
                window.location.href = '/TakeExam/Finish?takeId=' + $("#takeExamId").val() + '&timePassed=00:00:00';
            }
        }, 1000);
        checkTime.push(x);
    }
}

function StartTimer(checkTime, timePassed) {
    var startTime = Number(sessionStorage.getItem('startTime'));
    if (!startTime) {
        if (timePassed) {
            // Convert timePassed from HH:MM:SS format to milliseconds
            var timeParts = timePassed.split(':');
            var timeInMilliseconds = (+timeParts[0]) * 60 * 60 * 1000 + (+timeParts[1]) * 60 * 1000 + (+timeParts[2]) * 1000;
            startTime = new Date().getTime() - timeInMilliseconds;
        } else {
            startTime = new Date().getTime();
        }
        sessionStorage.setItem('startTime', startTime);
    }
    if (checkTime.length == 0) {
        var x = setInterval(function () {
            var now = new Date().getTime();
            var t = now - startTime;
            var hours = Math.floor((t % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((t % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((t % (1000 * 60)) / 1000);
            // Add a '0' in front of minutes and seconds if they are less than 10
            hours = hours < 10 ? '0' + hours : hours;
            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;

            if (hours != 0 || minutes != '00' || seconds != '00') {
                document.getElementById("timer").innerHTML = hours + ":" + minutes + ":" + seconds;
            }
        }, 1000);
        checkTime.push(x);
    }
}
