$(document).ready(function () {

    

    $(".regForm").validate({
        debug: false,
        rules: {
            FirstName: {
                required: true
            },
            LastName: {
                required: true
            },
            UserName: {
                required: true
            },
            Password: {
                required: true,
                minlength: 8
            },
            ConfirmPassword: {
                required: true,
                minlength: 8,
                equalTo: "#Password"
            }
        },
        messages: {

            FirstName: {
                required: "You must provide a first name"
            },
            LastName: {
                required: "You must provide a last name"
            },
            UserName: {
                required: "You must provide a username"
            },
            Password: {
                required: "You must provide a password",
                minlength: "Password must be atleast be 8 characters long"
            },
            ConfirmPassword: {
                required: "Your confirmation must be the same as the password",
                minlength: 8
            }
        }

    });
    $(".logForm").validate({
        debug: false,
        rules: {

            UserName: {
                required: true
            },
            Password: {
                required: true,
                minlength: 8
            },

        },
        messages: {

            UserName: {
                required: "You must provide a username"
            },
            Password: {
                required:  "You must provide a password",
                minlength: "Password must be atleast be 8 characters long"
            },

        }
    });
    $(".ingForm").validate({
        debug: false,
        rules: {

            Name: {
                required: true
            } 
        },
        messages: {
            Name: {
                required: "Please add an Ingredient"
            }
        }
    });
    $(".recForm").validate({
        debug: false,
        rules: {

            Name: {
                required: true
            },
            Description: {
                required: true
            },
            PrepTime: {
                required: true,
                NumbersOnly: true
            },
            CookTime: {
                required: true,
                NumbersOnly: true
            }
        },
        messages: {
            Name: {
                required: "Please add the name of the recipe"
            },
            Description: {
                required: "Please add a description of the recipe"
            },
            PrepTime: {
                required: "Please add the prep time for your recipe",
                NumbersOnly: "Please enter only numbers"
            },
            CookTime: {
                required: "Please add the cook time for your recipe",
                NumbersOnly: "Please enter only numbers"
            }
        }
    });
    $(".mealForm").validate({
        debug: false,
        rules: {

            Name: {
                required: true
            },
            Description: {
                required: true
            },
            RecipeSelection: {
                required: true
            }
        },
        messages: {
            Name: {
                required: "Please add the name of the meal"
            },
            Description: {
                required: "Please add a description of the meal"
            },
            RecipeSelection: {
                required: "Please add a recipe to the meal"
            }
        }
    });
    $.validator.addMethod("Onlywords", function (value, index) {
        const ValidChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";

        for (let i = 0; i < value.length; i++) {
            currentvalue = value[i];
            if (ValidChar.indexOf(currentvalue) < 0) {
                return false;
            }
        }
        return true;

    }, "Please enter only letters");
});