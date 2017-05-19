var includesPath = 'assets';

var config = {

    paths: {
        includes: includesPath
    },

    watch: {
        src: includesPath + "/scss/**/*.scss"
    },

    scss: [{
            src: includesPath + "/scss/main.scss",
            dest: includesPath + "/css"
    },
    {
        src: includesPath + "/scss/vendors/bootstrap.scss",
        dest: includesPath + "/css"
    },
    {
        src: includesPath + "/scss/vendors/font_awesome.scss",
        dest: includesPath + "/css"
    }]

};

module.exports = config;
