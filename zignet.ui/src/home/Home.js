import React, { Component } from 'react';
import SuiteResults from './SuiteResults'

class Home extends Component {
  constructor(props) {
    super(props);

    this.state = {
     latestSuiteResults: [],
     filter: this.props.filter
   }

   this.handleFilterChange = this.handleFilterChange.bind(this);
  }

  getData() {
    this.props.getResults(this.props.filter)
      .then(response => {
        this.setState({
          latestSuiteResults: response
        })
      })
      .catch(error => console.log('Failed to fetch latest suite results: ' + error));
  }

  componentDidMount() {
    this.getData();
    this.intervalId = setInterval(() => this.getData(), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  handleFilterChange(filter) {
    this.setState({
      filter: filter
    })
    this.getData();
  }

  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <SuiteResults suiteResults={this.state.latestSuiteResults} filter={this.state.filter} onFilterChange={this.handleFilterChange} />
      </div>
    );
  }
}

export default Home;
