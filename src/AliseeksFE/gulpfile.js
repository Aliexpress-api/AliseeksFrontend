/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass"),
    watch = require("gulp-watch");
    uglifycss = require('gulp-uglifycss'),
    pump = require('pump');

var paths = {
    webroot: "./wwwroot/"
};

gulp.task('default', function () {
    // place code for your default task here
});

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.sass = paths.webroot + "sass/**/*.scss";
paths.css = paths.webroot + "css/**/*.css";
paths.concatJsDest = paths.webroot + "js/site.js";
paths.concatJsDestMin = paths.webroot + "js/";
paths.concatCssDest = paths.webroot + "css/";
paths.concatCssDestMin = paths.webroot + "css/";

gulp.task("clean:js", function (cb) {
    return rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    return rimraf(paths.concatCssDest, cb);
});

gulp.task("css:js", function () {
    return rimraf(paths.concatCssDest);
});

gulp.task("compile:sass", function() {
    return gulp.src(paths.sass)
           .pipe(sass())
	   .pipe(concat('site.css'))
           .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task("watch:sass", function () {
    return watch(paths.sass, ['compile:sass']);
});

gulp.task("concat:js", function() {
    return gulp.src([paths.js])
        .pipe(concat(paths.concatJsDest))
        .pipe(gulp.dest("./"));
});

gulp.task("min:js", function (cb) {
  pump([
        gulp.src(paths.concatJsDest),
        uglify(),
	concat('site.min.js'),
        gulp.dest(paths.concatJsDestMin)
    ],
    cb
  );
});

gulp.task("min:css", function() {
    return gulp.src(paths.concatCssDest + "/site.css")
    .pipe(uglifycss({
      "maxLineLen": 80,
      "uglyComments": true
    }))
    .pipe(concat('site.min.css'))
    .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task("clean", ["clean:js", "clean:css"]);
gulp.task("build", ["clean", "compile:sass", "concat:js", "min:css", "min:js"]);
gulp.task("watch", ["watch:sass"]);
