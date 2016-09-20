/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass"),
    watch = require("gulp-watch"),
    uglifycss = require('gulp-uglifycss'),
    pump = require('pump'),
    imagemin = require('gulp-imagemin'),
    uncss = require('gulp-uncss'),
    nano = require('gulp-cssnano'),
    rename = require('gulp-rename');

var paths = {
    webroot: "./wwwroot/"
};

gulp.task('default', function () {
    // place code for your default task here
});

paths.js = paths.webroot + "dev/js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.sass = paths.webroot + "dev/sass/**/*.scss";
paths.css = paths.webroot + "css/**/*.css";
paths.images = paths.webroot + "dev/images/**/*";
paths.imagesDest = paths.webroot + "images";
paths.concatJsDest = paths.webroot + "js/**/*";
paths.concatJsDestMin = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/";
paths.concatCssDestMin = paths.webroot + "css/";

paths.uncss = paths.webroot + "css/site.css";
paths.uncssDest = paths.webroot + "css/home/";

gulp.task("clean:js", function (cb) {
    return rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    return rimraf(paths.concatCssDest, cb);
});

gulp.task("compile:sass", ["clean:css"], function(cb) {
        pump([
          gulp.src(paths.sass),
          sass(),
          concat('site.css'),
          gulp.dest(paths.concatCssDest)
        ],
      cb
    );
});

gulp.task("concat:js", ["clean:js"], function (cb) {
    pump([
          gulp.src(paths.js),
          concat("site.js"),
          gulp.dest(paths.webroot + "js/")
    ],
      cb
    );
});

gulp.task("uglify:js", ["concat:js"], function (cb) {
  pump([
        gulp.src(paths.webroot + "js/site.js"),
        uglify(),
        rename('site.min.js'),
        gulp.dest(paths.webroot + "js/")
    ],
    cb
  );
});

gulp.task("uglify:css", ["compile:sass"], function() {
    return gulp.src(paths.concatCssDest + "/site.css")
    .pipe(nano({
        discardComments: {
            removeAll: true
        }
    }))
    .pipe(rename('site.min.css'))
    .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task("min:images", function () {
    return gulp.src(paths.images)
    .pipe(imagemin())
    .pipe(gulp.dest(paths.imagesDest))
});

gulp.task("uncss:home", function () {
    return gulp.src(paths.uncss)
                .pipe(uncss({
                    html: ['http://www.aliseeks.com']
                }))
                .pipe(nano({
                    discardComments: {
                        removeAll: true
                    }
                }))
                .pipe(rename('home.min.css'))
                .pipe(gulp.dest(paths.uncssDest));
});

gulp.task("clean", ["clean:js", "clean:css"]);
gulp.task("build", ["uglify:js", "uglify:css", "min:images"]);
gulp.task("watch", ["watch:sass"]);
