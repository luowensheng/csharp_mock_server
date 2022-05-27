using Annotation;

[Controller]
public class SimpleController {

    [GetMapping(value="/index")]
    public string index(string item){
        return "Hello Index";
    }

    [GetMapping(value="/home/users/surla")]
    public string home([RequestParam(name = "home")] object param){
        return $"\"Hello Home\"+{ param}";
    }
}
