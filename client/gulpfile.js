var gulp = require("gulp");
var plugins = require("gulp-load-plugins")();
var typescript = require("typescript");

gulp.task("clean", require("del").bind(null, ["dist", "content/js", "main.js"]));

gulp.task("dtsm", function () {
    return gulp.src("dtsm.json").pipe(plugins.dtsm());
});

gulp.task("scripts:main", ["clean", "dtsm"], function () {
    var tsconfig = require("./tsconfig.json");
    var options = tsconfig.compilerOptions;
    options.typescript = typescript;
    return gulp.src(tsconfig.files)
        .pipe(plugins.typescript(options))
        .js.pipe(gulp.dest("./"));
});

gulp.task("scripts:content", ["clean", "dtsm"], function () {
    var options = require("./ts/tsconfig.json").compilerOptions;
    options.typescript = typescript;
    return gulp.src("ts/**/*.ts")
        .pipe(plugins.typescript(options))
        .js.pipe(gulp.dest("content/js"));
});

gulp.task("scripts", ["scripts:main", "scripts:content"]);

gulp.task("default", ["scripts"], function () {
    var through = require("through2");
    var electron = require("gulp-atom-electron");
    return gulp.src(["package.json", "main.js", "content/**/*", "node_modules/winjs/**/*", "bower_components/**/*"])
        .pipe(through.obj(function (file, enc, cb) {
            file.base = __dirname;
            cb(null, file);
        }))
        .pipe(electron({
            version: "0.28.1",
            platform: "win32",
            arch: "ia32",
            copyright: "©2015 azyobuzin"
        }))
        .pipe(gulp.dest("dist"));
});
