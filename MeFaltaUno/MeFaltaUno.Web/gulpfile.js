var config = require('./config');
var colors = require('colors');
var gulp = require('gulp');
var scss = require('gulp-scss');
var streamify = require('gulp-streamify');
var path = require('path');
var watch = require("gulp-watch");
var minifyCss = require('gulp-clean-css');
var sourcemaps = require('gulp-sourcemaps');
var source = require('vinyl-source-stream');
var merge = require('merge-stream');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

gulp.task('default', ['scss', 'watch']);

gulp.task('clean:compiledStyles', function () {
    return del(['/assets/css/*.css'], { force: true });
});

gulp.task('scss', ['clean:compiledStyles'], function () {

    var tasks = config.scss.map(function (element) {
        return gulp.src(element.src)
           .pipe(sourcemaps.init())
          .pipe(scss())
          .pipe(minifyCss())
          .pipe(sourcemaps.write())
          .pipe(gulp.dest(element.dest));
    });

    return merge(tasks);
});


gulp.task('watch', function () {

    gulp.watch(config.watch.src, ['scss']);
});