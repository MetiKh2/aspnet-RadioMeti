function AddMusicLike(id) {
    $.ajax({
        type: "POST",
        url: "/AddMovieLike/" + id,
        //data: {
        //    musicId: id
        //},
        dataType: "json",
        success: function (res) {
            if (res) ShowMessage("Congrats", "Like Added", "success")
            //else ShowMessage("Error", "Like No Added", "error")
        }
    });
};

function AddProdcastLike(id) {
    $.ajax({
        type: "POST",
        url: "/AddProdcastLike/" + id,
        //data: {
        //    musicId: id
        //},
        dataType: "json",
        success: function (res) {
            if (res) ShowMessage("Congrats", "Like Added", "success")
            //else ShowMessage("Error", "Like No Added", "error")
        }
    });
};

function AddVideoLike(id) {
    $.ajax({
        type: "POST",
        url: "/AddVideoLike/" + id,
        //data: {
        //    musicId: id
        //},
        dataType: "json",
        success: function (res) {
            if (res) ShowMessage("Congrats", "Like Added", "success")
            //else ShowMessage("Error", "Like No Added", "error")
        }
    });
};

