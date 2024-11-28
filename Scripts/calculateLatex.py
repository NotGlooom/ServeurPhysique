import sys
import json
from sympy import symbols
from sympy.parsing.latex import parse_latex
from sympy.parsing.latex.errors import LaTeXParsingError
import base64

def calculerLatex(json_base64):
    """
    :param json_base64: json qui contient le field expression et une liste de symboles encoder en base64
    :return:
    """
    
    # Décode l'entré
    json_string = base64.b64decode(json_base64).decode('utf-8')
    
    # Convertit l'entré JSON
    data = json.loads(json_string)
    expressionLatex = data['Expression']
    values = data['Symboles']
    
    # Extrait les symboles
    sym_names = values.keys()
    sym_list = symbols(" ".join(sym_names))
    
    # Dans le cas d'un seul symbole, le convertit en liste
    if not isinstance(sym_list, (list, tuple)):
        sym_list = [sym_list]
    sym_dict = dict(zip(sym_names, sym_list))
    
    # Convertit le latex en sympy, et gère les erreurs
    expression = parse_latex(expressionLatex)
    
    # Change les symboles pour leur valeurs
    expressionEvalue = expression.subs(sym_dict)
    
    # Calcul le résultat
    total = expressionEvalue.evalf(subs=values)
    return str(total)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        raise SystemExit("Error: Le script a besoin d'un argument.")
    else:
        print(calculerLatex(sys.argv[1]), end="")