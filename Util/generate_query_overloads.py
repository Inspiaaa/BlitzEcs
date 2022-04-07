"""
Automatically generates the QueryOverloads.cs file, as producing it is a very
repetitive (and therefore error-prone) task.

You can specify the maximum number of components that you can query for.
"""


from jinja2 import Environment, FileSystemLoader
import os


MAX_COMPONENT_COUNT = 5


def get_component_names(count):
    return [f"C{n}" for n in range(1, count + 1)]


def prefix(items, prefix):
    return [prefix + item for item in items]


local_folder = os.path.dirname(__file__)

loader = FileSystemLoader(searchpath=local_folder)
env = Environment(loader=loader)

env.globals["get_component_names"] = get_component_names
env.filters["prefix"] = prefix

template = env.get_template("query_overload_template.jinja2")
code = template.render(max_component_count = MAX_COMPONENT_COUNT)

with open(os.path.join(local_folder, "output", "QueryOverloads.cs"), "w") as file:
    file.write(code)
