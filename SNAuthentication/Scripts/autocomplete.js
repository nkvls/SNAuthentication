$(document).ready(function () { 
    $('*[data-autocomplete-url]') 
        .each(function () { 
            $(this).autocomplete({ 
                source: $(this).data('autocomplete-url'),
                minLength: 1
            }); 
        }); 
});
